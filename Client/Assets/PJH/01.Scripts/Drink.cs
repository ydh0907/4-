using System;
using UnityEngine;

namespace PJH
{
    public class Drink : MonoBehaviour
    {
        private Player _owner;
        [SerializeField] private float _bounceOff = 4;
        [SerializeField] private float _moveYSpeed = 3;
        [SerializeField] private float _maxYPos = 0.05f;
        private Vector3 _originPos;

        private void Awake()
        {
            _originPos = transform.position;
            _owner = transform.root.GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            Vector3 pos = transform.position;
            pos.y = _originPos.y;
            pos.y += _maxYPos * Mathf.Sin(Time.time * _moveYSpeed);
            transform.position = pos;
        }

        public void ApplyDamage(int damage)
        {
            _owner.AddForce((-_owner.Model.transform.forward + new Vector3(0, 1.5f, 0)) * _bounceOff);
            _owner.ApplyDamage(damage);
        }
    }
}