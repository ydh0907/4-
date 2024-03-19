using System;
using Unity.Netcode;
using UnityEngine;

namespace PJH
{
    public class Drink : NetworkBehaviour
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
            if (!IsOwner) return;
            Vector3 pos = transform.localPosition;
            pos.y = _originPos.y;
            pos.y += _maxYPos * Mathf.Sin(Time.time * _moveYSpeed);
            transform.localPosition = pos;
        }

        [ServerRpc]
        public void ApplyDamageServerRpc(int damage, float bounceOff)
        {
            ApplyDamageClientRpc(damage, bounceOff);
        }

        [ClientRpc]
        public void ApplyDamageClientRpc(int damage, float bounceOff)
        {
            _owner.AddForce((-_owner.Model.transform.forward + new Vector3(0, 1.5f, 0)) * bounceOff);
            _owner.ApplyDamage(damage);
        }
    }
}