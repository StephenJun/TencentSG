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

	private void Start()
	{
		GetComponent<CameraFilterPack_3D_Fog_Smoke>().enabled = false;
		GetComponent<CameraFilterPack_FX_EarthQuake>().enabled = false;
	}


	private void LateUpdate()
    {
        //cam.LookAt(target);

        Vector3 targetPosition = target.position + Vector3.up * height - Vector3.forward * distance;

        cam.position = Vector3.Lerp(cam.position, targetPosition , Time.deltaTime * speed);
    }


	public void SetSmokeShaderActive(bool newActive)
	{
		GetComponent<CameraFilterPack_3D_Fog_Smoke>().enabled = newActive;
	}

	public void SetShockShaderActive(bool newAction)
	{
		GetComponent<CameraFilterPack_FX_EarthQuake>().enabled = newAction;
	}

	public void SetRGBShaderActive(Color newColor, float duration)
	{
		DOTween.To(()=> GetComponent<CameraFilterPack_Color_RGB>().ColorRGB, x => GetComponent<CameraFilterPack_Color_RGB>().ColorRGB = x, newColor, duration);
	}

}
