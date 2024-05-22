using DH;
using Unity.Netcode;

namespace PJH
{
    public partial class Player
    {
        private void Sprint(bool value)
        {
            var sprintConditions = (_input.sqrMagnitude > 0.1f && IsGrounded &&
                                    !(IsStrafing && !strafeSpeed.walkByDefault && (_horizontalSpeed >= 0.5 ||
                                        _horizontalSpeed <= -0.5 ||
                                        _verticalSpeed <= 0.1f)));

            if (value && sprintConditions)
            {
                if (_input.sqrMagnitude > 0.1f)
                {
                    if (IsGrounded && _useContinuousSprint)
                    {
                        IsSprinting = !IsSprinting;
                    }
                    else if (!IsSprinting)
                    {
                        IsSprinting = true;
                    }
                }
                else if (!_useContinuousSprint && IsSprinting)
                {
                    IsSprinting = false;
                }
            }
            else if (IsSprinting)
            {
                IsSprinting = false;
            }

            SetSprintStateServerRpc(IsSprinting);
        }

        [ServerRpc]
        private void SetSprintStateServerRpc(bool isSprint)
        {
            IsSprinting = isSprint;
        }

        private void Strafe(bool isStrafe)
        {
            IsStrafing = isStrafe;
        }

        private void Jump()
        {
            _jumpCounter = _jumpTimer;
            IsJumping = true;

            if (_input.sqrMagnitude < 0.1f)
                _animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                _animator.CrossFadeInFixedTime("JumpMove", .2f);
        }
    }
}