using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform posFollow;
    [SerializeField] Transform angFollow;

    private void Awake()
    {
        angFollow = Camera.main.transform;
    }

    private void Update()
    {
        transform.position = posFollow.position;
        transform.eulerAngles = new Vector3(0, angFollow.eulerAngles.y, 0);
    }
}
