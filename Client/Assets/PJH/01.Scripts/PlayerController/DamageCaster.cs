using UnityEngine;

namespace PJH
{
    public class DamageCaster : MonoBehaviour
    {
        [SerializeField] AudioClip hitSound;
        [SerializeField] private int _damage = 10;
        [SerializeField] private int _mentosDamage = 40;
        [SerializeField] private float _bounceOff = 4;
        private Collider _collider;
        private GameObject mentosVisual;
        private Player _owner;

        public int Damage => _damage;

        public bool isMentosMode { get; private set; } = false;

        private void Start()
        {
            mentosVisual = transform.Find("MentosVisual").gameObject;
            _collider = GetComponent<Collider>();
            EnableCollider(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_owner)
            {
                _owner = transform.root.GetComponent<Player>();

                if (!_owner)
                {
                    Debug.Log("Owner Not Found");
                    return;
                }
            }
            if (transform.root.GetHashCode() == other.transform.root.GetHashCode()) return;
            if (!_owner.IsServer) return;

            if (other.TryGetComponent(out Player player))
            {
                FaintEnemy(player);
                return;
            }

            if (other.TryGetComponent(out Drink drink))
            {
                GiveDamageToEnemy(drink);
            }
        }

        public void FaintEnemy(Player enemy)
        {
            enemy.Faint();
            EnableCollider(false);
            SoundManager.Instance.Play(hitSound);
        }

        public void GiveDamageToEnemy(Drink drink)
        {
            int damage = isMentosMode ? _mentosDamage : _damage;
            Debug.Log(damage);

            drink.ApplyDamage(damage, _bounceOff, _owner.transform.position, _owner);
            EnableCollider(false);
            SoundManager.Instance.Play(hitSound);
            if (isMentosMode)
            {
                DisableMentosMode();
                _owner.DisableMentosClientRpc();
            }
        }

        public void EnableCollider(bool enable)
        {
            if (_owner == null) return;
            if (!_owner.IsServer) return;

            _collider.enabled = enable;
        }

        public void EnableMentosAttack()
        {
            isMentosMode = true;
            mentosVisual.SetActive(true);
        }

        public void DisableMentosMode()
        {
            isMentosMode = false;
            mentosVisual.SetActive(false);
        }
    }
}