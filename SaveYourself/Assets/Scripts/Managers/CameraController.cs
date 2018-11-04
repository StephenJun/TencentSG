using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{
    public Transform cam;
    public Transform target;
    public float distance;
    public float height;
    public float speed = 100;

	public Component smokeScript;
	public Component shockScript;

	private void Start()
	{
		smokeScript.GetComponent<CameraFilterPack_3D_Fog_Smoke>().enabled = false;
		shockScript.GetComponent<CameraFilterPack_FX_EarthQuake>().enabled = false;
	}


	private void LateUpdate()
    {
        //cam.LookAt(target);

        Vector3 targetPosition = target.position + Vector3.up * height - Vector3.forward * distance;

        cam.position = Vector3.Lerp(cam.position, targetPosition , Time.deltaTime * speed);
    }


	public void SetSmokeShaderActive(bool newActive)
	{
		smokeScript.GetComponent<CameraFilterPack_3D_Fog_Smoke>().enabled = newActive;
	}

	public void SetShockShaderActive(bool newAction)
	{
		smokeScript.GetComponent<CameraFilterPack_FX_EarthQuake>().enabled = newAction;
	}

}
