using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DH
{
    public class PlayerMovement : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerInput _playerInput;
        private CharacterController _characterController;

        [Header("Movement Variable")]
        [SerializeField] private float _speed = 7;
        [SerializeField] private float _gravity = -9.8f;
        [SerializeField] private float _jumpForce = 12;

        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            _playerInput.MovementInputAction += HandleMovementInput;
        }

        private void OnDestroy()
        {
            _playerInput.MovementInputAction -= HandleMovementInput;
        }

        private void HandleMovementInput(Vector3 direction)
        {
            direction.Normalize();
            _velocity = direction * _speed;
        }

        private void FixedUpdate()
        {
            CalculateGravity();
            SetPlayerDirection();
            ApplyCurrentVelocity();
        }

        private void CalculateGravity()
        {
            if (_characterController.isGrounded)
            {
                _velocity.y = -0.03f;
            }
            else
            {
                _velocity.y += _gravity * Time.fixedDeltaTime;
            }
        }
        
        private void SetPlayerDirection()
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.LookRotation(new Vector3(_velocity.x, 0, _velocity.z), transform.up), 
                Time.fixedDeltaTime);
        }

        private void ApplyCurrentVelocity()
        {
            _characterController.Move(transform.position + _velocity * Time.deltaTime);
        }
    }
}
