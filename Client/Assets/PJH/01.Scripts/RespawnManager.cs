using DH;
using System.Collections;
using UnityEngine;

namespace PJH
{
    public class RespawnManager : MonoSingleton<RespawnManager>
    {
        [SerializeField] private float _respawnTime;


        public void Respawn(Player player)
        {
            StartCoroutine(RespawnCoroutine(player));
        }

        private IEnumerator RespawnCoroutine(Player player)
        {
            yield return new WaitForSeconds(_respawnTime);
            if (NetworkGameManager.Instance.IsOnGame.Value)
                player.Respawn();
        }
    }
}