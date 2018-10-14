using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public Transform cam;
    public Transform target;
    public float distance;
    public float height;
    public float speed = 100;
    private void LateUpdate()
    {
        cam.LookAt(target);

        Vector3 targetPosition = target.position + Vector3.up * height - target.forward * distance;

        cam.position = Vector3.Lerp(cam.position, targetPosition , Time.deltaTime * speed);
    }

}
