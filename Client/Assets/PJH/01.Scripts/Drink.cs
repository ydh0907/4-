using System;
using Unity.Netcode;
using UnityEngine;

namespace PJH
{
    public class Drink : MonoBehaviour
    {
        private Player _owner;
        [SerializeField] private float _moveYSpeed = 3;
        [SerializeField] private float _maxYPos = 0.05f;
        private Vector3 _originPos;

        private void Awake()
        {
            _originPos = transform.localPosition;
            _owner = transform.root.GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            Vector3 pos = transform.localPosition;
            pos.y = _originPos.y;
            pos.y += _maxYPos * Mathf.Sin(Time.time * _moveYSpeed);
            transform.localPosition = pos;
        }
        
        public void ApplyDamage(int damage, float bounceOff, Vector3 position, Player harmer)
        {
            _owner.ApplyDamage(damage, position, harmer, bounceOff);
        }
    }
}