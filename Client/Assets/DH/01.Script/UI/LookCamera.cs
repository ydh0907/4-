using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Camera main;

    private void Awake()
    {
        main = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - main.transform.position, Vector3.up);
    }
}
