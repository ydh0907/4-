using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandFollow : NetworkBehaviour
{
    Transform target = null;


    private void Update()
    {
        if (target)
        {
            transform.position = target.position;
        }
        else
        {
            target = transform.Find("Hand.L");
        }
    }
}
