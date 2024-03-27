using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace PJH
{
    public class DamageCaster : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _bounceOff = 4;
        private Collider _collider;

        private Player _owner;

        private void Start()
        {
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
                player.Faint();
                EnableCollider(false);
                Debug.Log("Hit Player");
                return;
            }

            if (other.TryGetComponent(out Drink drink))
            {
                drink.ApplyDamage(_damage, _bounceOff, _owner.transform.position);
                EnableCollider(false);
                Debug.Log("Hit Cola");
            }
        }

        public void EnableCollider(bool enable)
        {
            if (_owner == null) return;
            if (!_owner.IsServer) return;

            _collider.enabled = enable;
            Debug.Log("On Collider " + enable);
        }
    }
}