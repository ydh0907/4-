using AH;
using DH;
using GM;
using HB;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace PJH
{
    public partial class Player : NetworkBehaviour
    {
        [SerializeField] private AudioClip bodyHitSound;
        [SerializeField] private AudioClip drinkHitSound;
        [SerializeField] private ParticleSystem bodyHitParticle;
        [SerializeField] private ParticleSystem drinkHitParticle;
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private float hpDecreaseTimeMultiplier = 1f;
        public GameObject HealthBar;

        private Player harmer;

        public void StartInit()
        {
            Init();
            Join();
            SetStart();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            UnJoin();
        }
        private void SetStart()
        {
            if (_rigidbody.isKinematic) _rigidbody.isKinematic = false;
        }

        private void Join()
        {
            if (!IsOwner) return;
            _inputReader.AttackEvent += HandleAttackEvent;
            _inputReader.MovementEvent += HandleMovementEvent;
            _inputReader.JumpEvent += HandleJumpEvent;
            _inputReader.RunEvent += HandleSprintEvent;
            _inputReader.UseMentosEvent += UseMentos;
        }

        private void UnJoin()
        {
            if (!IsOwner) return;
            _inputReader.AttackEvent -= HandleAttackEvent;
            _inputReader.MovementEvent -= HandleMovementEvent;
            _inputReader.JumpEvent -= HandleJumpEvent;
            _inputReader.RunEvent -= HandleSprintEvent;
            _inputReader.UseMentosEvent -= UseMentos;
        }

        private void Update()
        {
            if (!IsOwner) return;

            UpdateAnimator();
            CheckGround();
            CheckSlopeLimit();
            UpdateMoveDirection(_mainCamera.transform);

            ControlRotationType();
        }

        private void OnEnable()
        {
            GetComponent<ClientNetworkTransform>().Interpolate = true;
        }

        private void OnDisable()
        {
            GetComponent<ClientNetworkTransform>().Interpolate = false;
        }

        private void DecreaseHpByTime()
        {
            if (NetworkGameManager.Instance)
                if (NetworkGameManager.Instance.IsOnGame.Value)
                    ApplyDamage(Time.fixedDeltaTime * (IsSprinting ? 2 : 1) * hpDecreaseTimeMultiplier, Vector3.zero, harmer, 0, false);
        }

        [ClientRpc]
        public void PlaySoundAndEffectClientRpc(bool isHitDrink)
        {
            Debug.Log("PlaySoundAndEffectClientRpc");

            if (isHitDrink)
            {
                SoundManager.Instance.Play(drinkHitSound);
                Instantiate(drinkHitParticle, transform.position, Quaternion.identity);
            }
            else
            {
                SoundManager.Instance.Play(bodyHitSound);
                Instantiate(bodyHitParticle, transform.position, Quaternion.identity);
            }
        }

        private void FixedUpdate()
        {
            if (IsServer)
                DecreaseHpByTime();

            if (!IsOwner) return;

            ControlJumpBehaviour();
            ControlLocomotionType();
            AirControl();
        }

        #region Handle event

        private void HandleMovementEvent(Vector3 input)
        {
            _input = input;
        }

        private void HandleJumpEvent()
        {
            if (IsGrounded && GroundAngle() < _slopeLimit && !IsJumping && !StopMove)
                Jump();
        }

        private void HandleSprintEvent(bool isSprint)
        {
            Sprint(isSprint);
        }

        private void HandleAttackEvent()
        {
            if (IsAttacking) return;
            if (_isDead) return;
            if (StopMove) return;
            IsAttacking = true;
            _animator.CrossFadeInFixedTime("Attack", .1f);
        }

        #endregion

        public void FinishAttack()
        {
            IsAttacking = false;
        }

        #region Debugging

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_capsuleCollider == null) _capsuleCollider = GetComponent<CapsuleCollider>();
            float radius = _capsuleCollider.radius * .9f;
            Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos + (-Vector3.up * (_capsuleCollider.radius + _groundMaxDistance)), radius);
            Gizmos.color = Color.white;
        }
#endif

        #endregion


        public void ApplyDamage(float damage, Vector3 position, Player harmer, float bounceOff, bool changeState = true)
        {
            Vector3 dir = transform.position - position;
            dir.Normalize();

            this.harmer = harmer;

            if (!IsAttacking && changeState)
                _animator.Play("Hit");

            if (_health != null)
            {
                _health.TakeDamage(damage);
                if (_health.CurrentHealth <= Mathf.Epsilon) Death();
                else if (bounceOff > 0) AddForceClientRpc(bounceOff * dir, OwnerClientId);
            }
        }

        [ClientRpc]
        private void AddForceClientRpc(Vector3 force, ulong id)
        {
            if (OwnerClientId == id)
                AddForce(force);

            _animator.Play("Hit");
        }

        public void Faint()
        {
            _animator.Play("Hit");
            Debug.Log("Faint 1f");
            _rigidbody.velocity = Vector3.zero;
            _lockMovement = true;
            _lockRotation = true;
            StopMove = true;
            _inputMagnitude = 0;
            StartCoroutine(Wait(1f, () =>
            {
                StopMove = false;
                _lockMovement = false;
                _lockRotation = false;
                IsAttacking = false;
            }));
            FaintClientRpc();
        }

        [ClientRpc]
        private void FaintClientRpc()
        {
            _animator.Play("Hit");
            Debug.Log("Faint 1f");
            _rigidbody.velocity = Vector3.zero;
            _lockMovement = true;
            _lockRotation = true;
            StopMove = true;
            _inputMagnitude = 0;
            StartCoroutine(Wait(1f, () =>
            {
                StopMove = false;
                _lockMovement = false;
                _lockRotation = false;
                IsAttacking = false;
            }));
        }

        private IEnumerator Wait(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        private void Death()
        {
            if (_isDead) return;

            if (harmer)
            {
                NetworkGameManager.Instance.PlayerKillCount(harmer.OwnerClientId);
            }

            RespawnManager.Instance.Respawn(this);
            DeathClientRpc();

            GetComponent<ClientNetworkTransform>().Interpolate = false;
        }

        [ClientRpc]
        private void DeathClientRpc()
        {
            _isDead = true;
            gameObject.SetActive(false);

            if (IsOwner && NetworkGameManager.Instance.IsOnGame.Value)
                FindObjectOfType<IngameUIToolkit>().ResurrectionCounter(() =>
                {
                    if (DamageCaster.isMentosMode)
                        IngameUIToolkit.instance.ChangeMantosAttack();
                });

            Instantiate(explosionPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            SoundManager.Instance.Play(explosionSound);

            GetComponent<ClientNetworkTransform>().Interpolate = false;
        }

        public void Respawn()
        {
            _health.Reset();

            IsAttacking = false;
            IsJumping = false;
            _isDead = false;
            IsSprinting = false;
            IsStrafing = false;
            _input = Vector3.zero;
            _lockMovement = false;
            _lockRotation = false;
            StopMove = false;
            gameObject.SetActive(true);

            RespawnClientRpc();
            GetComponent<ClientNetworkTransform>().Interpolate = true;
        }

        [ClientRpc]
        public void RespawnClientRpc()
        {
            if (IsOwner)
            {
                transform.position = MapManager.Instance.GetSpawnPosition();
            }

            IsAttacking = false;
            IsJumping = false;
            _isDead = false;
            IsSprinting = false;
            IsStrafing = false;
            _input = Vector3.zero;
            _lockMovement = false;
            _lockRotation = false;
            StopMove = false;
            gameObject.SetActive(true);

            GetComponent<ClientNetworkTransform>().Interpolate = true;
        }

        public async void AddForce(Vector3 dir)
        {
            StopMove = true;
            _lockMovement = true;
            _lockRotation = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(dir, ForceMode.Impulse);
            await Task.Delay(500);
            StopMove = false;
            _lockMovement = false;
            _lockRotation = false;
            IsAttacking = false;
        }
    }
}