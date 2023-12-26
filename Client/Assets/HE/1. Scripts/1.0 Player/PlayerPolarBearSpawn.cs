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
            // *����
            // ���� �ð��� 2��, �ϱذ� ��ȯ �ð��� 5�ʶ� �Ͽ��� ��
            // Player�� 3�� ���� ������ �ִٰ� �����Ͽ���
            // ���ǵ� Ÿ�ǵ� �������� ���� �ð��� �� 5�� �̴� �ϱذ��� ��ȯ�Ѵ�.
            if (PlayerMovement.RB.velocity != Vector3.zero)
            {
                CurrentTime = 0;
            }
            else
            {
                CurrentTime += Time.deltaTime;
                if (CurrentTime >= SPAWN_TIME)
                {
                    // �ϱذ��� �����ϴ� ��ũ��Ʈ ����
                }
            }
        }

    }


}

// �̰� �÷��̾� �����Ʈ�� �̤Ǥ�����