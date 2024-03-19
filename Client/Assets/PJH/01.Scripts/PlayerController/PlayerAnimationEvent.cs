using System;
using PJH;
using Unity.Netcode;
using UnityEngine;


public class PlayerAnimationEvent : NetworkBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform?.parent?.GetComponent<Player>();
        if (!_player) enabled = false;
    }

    private void FinishAttack()
    {
        if (!IsOwner) return;
        _player.FinishAttack();
    }

    private void EnableCollider(int enable)
    {
        if (!IsOwner) return;
        _player.DamageCaster.EnableCollider(Convert.ToBoolean(enable));
    }
}