using UnityEngine;

namespace PJH
{
    public class DamageCaster : MonoBehaviour
    {
        [SerializeField] private int _damage;

        private Collider _collider;


        private void Awake()
        {
            _collider = GetComponent<Collider>();
            EnableCollider(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.Faint();
                EnableCollider(false);
                return;
            }

            if (other.TryGetComponent(out Drink drink))
            {
                drink.ApplyDamage(_damage);
                EnableCollider(false);
            }
        }

        public void EnableCollider(bool enable)
        {
            _collider.enabled = enable;
        }
    }
}