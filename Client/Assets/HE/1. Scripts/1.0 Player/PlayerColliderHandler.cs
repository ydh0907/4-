using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ITEMMENTOS"))
        {
            // PLAYER 밑 오브젝트 풀링으로 미리 만들어 둔 멘토스 하나를 온
            // 멘토스가 하나라도 온 되어 있다면 켄토스 사용 및 교체 가능
            //Vector2 hitDirection = collision.gameObject.transform.position - transform.position;
            //hitDirection.Normalize();
            //iDamageble.Damage(_attackDamageAmount, hitDirection);

            //dfsdfs
        }
    }
}
