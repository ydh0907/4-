using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ITEMMENTOS"))
        {
            // PLAYER �� ������Ʈ Ǯ������ �̸� ����� �� ���佺 �ϳ��� ��
            // ���佺�� �ϳ��� �� �Ǿ� �ִٸ� ���佺 ��� �� ��ü ����
            //Vector2 hitDirection = collision.gameObject.transform.position - transform.position;
            //hitDirection.Normalize();
            //iDamageble.Damage(_attackDamageAmount, hitDirection);

            //dfsdfs
        }
    }
}
