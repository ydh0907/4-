using System;
using PJH;
using UnityEngine;


public class PlayerAnimationEvent : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    private void FinishAttack()
    {
        _player.FinishAttack();
    }
}