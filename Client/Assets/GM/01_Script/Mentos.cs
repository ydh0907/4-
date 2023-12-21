using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class Mentos : MonoBehaviour
    {
        private bool isUp = true;
        private void Update()
        {
            if (transform.position.y >= 0.7f)
            {
                isUp = false;
            }
            else if (transform.position.y <= 0.4f)
            {
                isUp = true;
            }

            Vector3 pos = transform.position;
            if (isUp)
            {
                pos.y += Time.deltaTime / 5;
            }
            else
            {
                pos.y -= Time.deltaTime / 5;
            }
            transform.position = pos;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Player"))
            {
                Destroy(gameObject);
                Debug.Log("���佺 ������");
            }
        }
    }
}
