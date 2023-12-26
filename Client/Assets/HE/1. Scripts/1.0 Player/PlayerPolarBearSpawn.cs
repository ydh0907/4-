using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HB;

namespace HE
{
    public class PlayerPolarBearSpawn : MonoBehaviour
    {
        private PlayerMovement PlayerMovement;

        public float CurrentTime { get; private set; }

        private const float SPAWN_TIME = 5f;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            // *주의
            // 기절 시간이 2초, 북극곰 소환 시간이 5초라 하였을 때
            // Player가 3초 동안 가만히 있다가 기절하여도
            // 자의든 타의든 움직이지 않은 시간이 총 5초 이니 북극곰을 소환한다.
            if (PlayerMovement.RB.velocity != Vector3.zero)
            {
                CurrentTime = 0;
            }
            else
            {
                CurrentTime += Time.deltaTime;
                if (CurrentTime >= SPAWN_TIME)
                {
                    // 북극곰을 생성하는 스크립트 연결
                }
            }
        }

    }


}

// 이거 플레이어 무브먼트로 이ㅗㅇ시켜