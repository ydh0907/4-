using UnityEngine;

namespace PJH
{
    public class DamageCaster : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _bounceOff = 4;
        private Collider _collider;


        private Player _owner;

        private void Awake()
        {
            _owner = transform.root.GetComponent<Player>();
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
                drink.ApplyDamage(_damage, _bounceOff);
                _owner.AddForce((-_owner.Model.transform.forward + new Vector3(0, 1.5f, 0)) * _bounceOff);
                EnableCollider(false);
            }
        }

        public void EnableCollider(bool enable)
        {
            _collider.enabled = enable;
        }
    }
}