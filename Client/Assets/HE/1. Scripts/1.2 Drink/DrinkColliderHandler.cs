using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkColliderHandler : MonoBehaviour
{
    private Fist Fist;

    private void Awake()
    {
        Fist = GetComponentInParent<Fist>();
    }

    // 여기다 하는게 아니다!!!!!!!!! 공격하는 주체에 해야하는거 .. 맞는 ㄴㅁ이 아니라.... 처음부터 다 갈기 !!!!!
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageble iDamageble = other.gameObject.GetComponent<IDamageble>();

        if (other.gameObject.layer == LayerMask.NameToLayer("ITEMMENTOS"))
        {
            // PLAYER 밑 오브젝트 풀링으로 미리 만들어 둔 멘토스 하나를 온
            // 멘토스가 하나라도 온 되어 있다면 켄토스 사용 및 교체 가능
            //Vector2 hitDirection = collision.gameObject.transform.position - transform.position;
            //hitDirection.Normalize();
            //iDamageble.Damage(_attackDamageAmount, hitDirection);
        }
        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("FIST"))
        {
            Debug.Log("맞았어");
            Vector3 hitDirection = other.gameObject.transform.position - transform.position;
            iDamageble.Damage(Fist._attackDamageAmount, hitDirection);
        }
        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("MENTOS"))
        {

        }
    }
}
