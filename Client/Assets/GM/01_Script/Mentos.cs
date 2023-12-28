using HB;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GM
{
    public class Mentos : MonoBehaviour
    {
        private bool isUp = true;

        private void Update()
        {
            if (transform.position.y >= 0.69f)
            {
                isUp = false;
            }
            else if (transform.position.y <= 0.41f)
            {
                isUp = true;
            }

            Vector3 pos = transform.position;
            if (isUp)
            {
                if (transform.position.y >= 0.55f)
                    pos.y = Mathf.Lerp(transform.position.y, 0.7f, Time.deltaTime);
                else
                    pos.y += Time.deltaTime / 5;
            }
            else
            {
                if (transform.position.y <= 0.55f)
                    pos.y = Mathf.Lerp(transform.position.y, 0.4f, Time.deltaTime);
                else
                    pos.y -= Time.deltaTime / 5;
            }
            transform.position = pos;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.tag == "Player" || other.gameObject.layer == 8)
            {
                Destroy(gameObject);
            }
        }
    }
}
