using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PJH
{
    [CreateAssetMenu(menuName = "SO/Input Reader", fileName = "Input Reader")]
    public class InputReader : ScriptableObject, Control.IPlayerActions
    {
        public event Action<Vector3> MovementEvent;
        public event Action JumpEvent;
        public event Action<bool> RunEvent;
        public event Action AttackEvent;
        private Control _control;

        private void OnEnable()
        {
            _control = new Control();
            _control.Player.SetCallbacks(this);
            _control.Player.Enable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector3 input = context.ReadValue<Vector2>();
            input = new Vector3(input.x, 0, input.y);
            MovementEvent?.Invoke(input);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
             if (context.started) JumpEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            AttackEvent?.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started) RunEvent?.Invoke(true);
            if (context.canceled) RunEvent?.Invoke(false);
        }
    }
}