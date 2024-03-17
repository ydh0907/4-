using UnityEngine;

namespace PJH
{
    public partial class Player
    {
        public void ControlAnimatorRootMotion()
        {
            if (!this.enabled) return;

            if (_inputSmooth == Vector3.zero && UseRootMotion)
            {
                transform.position = _animator.rootPosition;
                _model.transform.rotation = _animator.rootRotation;
            }

            // if (UseRootMotion)
            //     MoveCharacter(_moveDirection);
        }
    }
}