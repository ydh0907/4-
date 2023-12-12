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

    // ����� �ϴ°� �ƴϴ�!!!!!!!!! �����ϴ� ��ü�� �ؾ��ϴ°� .. �´� ������ �ƴ϶�.... ó������ �� ���� !!!!!
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageble iDamageble = other.gameObject.GetComponent<IDamageble>();

        if (other.gameObject.layer == LayerMask.NameToLayer("ITEMMENTOS"))
        {
            // PLAYER �� ������Ʈ Ǯ������ �̸� ����� �� ���佺 �ϳ��� ��
            // ���佺�� �ϳ��� �� �Ǿ� �ִٸ� ���佺 ��� �� ��ü ����
            //Vector2 hitDirection = collision.gameObject.transform.position - transform.position;
            //hitDirection.Normalize();
            //iDamageble.Damage(_attackDamageAmount, hitDirection);
        }
        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("FIST"))
        {
            Debug.Log("�¾Ҿ�");
            Vector3 hitDirection = other.gameObject.transform.position - transform.position;
            iDamageble.Damage(Fist._attackDamageAmount, hitDirection);
        }
        if (iDamageble != null && other.gameObject.layer == LayerMask.NameToLayer("MENTOS"))
        {

        }
    }
}
