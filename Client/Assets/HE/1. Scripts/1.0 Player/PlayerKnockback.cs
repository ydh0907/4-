using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HE
{

    public class PlayerKnockback : MonoBehaviour
    {
        #region COMPONENTS
        private Rigidbody RB;
        #endregion

        #region STATE PARAMETERS
        public bool IsBegingKnockedBack { get; private set; }
        #endregion

        #region KNOCKBACK 
        [Header("Knockback")]
        [SerializeField] private float _knockbackTime;
        [SerializeField] private float _hitDirectionForce;
        [SerializeField] private float _constForce;
        [SerializeField] private float _inputForce;

        [SerializeField] private AnimationCurve KnockbackForceCurve;
        #endregion

        private void Awake()
        {
            RB = GetComponent<Rigidbody>();
        }

        public void StartKnockback(Vector3 hitDirection, Vector3 constantForceDirection, float inputDirection)
        {
            StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
        }

        private IEnumerator KnockbackAction(Vector3 hitDirection, Vector3 constantForceDirection, float inputDirection)
        {
            IsBegingKnockedBack = true;

            Vector3 hitForce;
            Vector3 constantForce;
            Vector3 knockbackForce;
            Vector3 combinedForce;

            float time = 0f;

            constantForce = constantForceDirection * _constForce;

            float elapsedTime = 0f;
            while (elapsedTime < _knockbackTime)
            {
                // Iterate the timer
                elapsedTime += Time.fixedDeltaTime;
                time += Time.fixedDeltaTime;

                // Update hitForce
                hitForce = hitDirection * _hitDirectionForce * KnockbackForceCurve.Evaluate(time);

                // Combine hitForce and constantForce
                knockbackForce = hitForce + constantForce;

                // Comnine knockback forec with input force
                if (inputDirection != 0)
                {
                    combinedForce = knockbackForce + new Vector3(inputDirection * _inputForce, 0f);
                }
                else
                {
                    combinedForce = knockbackForce;
                }

                // Apply knockback
                RB.velocity = combinedForce;

                yield return new WaitForFixedUpdate();
            }
            IsBegingKnockedBack = false;
        }
    }
}