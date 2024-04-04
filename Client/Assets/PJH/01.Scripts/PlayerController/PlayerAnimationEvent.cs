using PJH;
using System;
using UnityEngine;


public class PlayerAnimationEvent : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform?.parent?.GetComponent<Player>();
        if (!_player) enabled = false;
    }

    private void FinishAttack()
    {
        _player.FinishAttack();
    }

    private void EnableCollider(int enable)
    {
        if (!_player) return;
        if (!_player.IsServer) return;

        _player.DamageCaster.EnableCollider(Convert.ToBoolean(enable));
    }
}