using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPrsonCMA : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    Vector2 mousInput;

    float spd = 1;

    private void LateUpdate()
    {
        RotatCMA();
    }

    private void RotatCMA()
    {
        mousInput.x = Input.GetAxis("Mouse X");
        mousInput.y = Input.GetAxis("Mouse Y");

        if (mousInput.magnitude != 0)
        {
            Quaternion quaternion = followTransform.rotation;
            quaternion.eulerAngles = new Vector3(quaternion.eulerAngles.x + mousInput.y * spd, quaternion.eulerAngles.y + mousInput.x * spd, quaternion.eulerAngles.z);
            followTransform.rotation = quaternion;
        }
    }
}
