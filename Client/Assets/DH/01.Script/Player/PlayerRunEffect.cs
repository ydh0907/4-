using System.Collections.Generic;
using PJH;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerRunEffect : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    private Player owner;

    private bool _playing;

    private void Awake()
    {
        owner = GetComponent<Player>();
    }

    private void Update()
    {
        if (owner.IsSprinting && !owner.IsJumping && owner.IsGrounded)
        {
            if (!_playing)
            {
                _playing = true;
                effect.Play();
            }
        }
        else if (_playing)
        {
            _playing = false;

            effect.Stop();
        }
    }
}