using UnityEngine;
using Karin.PoolingSystem;


namespace Karin
{
    [RequireComponent(typeof(PoolManager))]
    [RequireComponent(typeof(SoundManager))]
    public class GameManager : MonoBehaviour
    {
        public PoolManager PoolManager;
        public SoundManager SoundManager;

        private void Awake()
        {
            PoolManager = GetComponent<PoolManager>();
            SoundManager = GetComponent<SoundManager>();
        }
    }
}
