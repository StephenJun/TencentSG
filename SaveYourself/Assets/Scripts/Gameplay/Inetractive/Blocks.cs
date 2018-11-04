using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : InteractiveObject {

	private void Start()
	{
		OnInteractive += delegate {
			PlaybackManager.Instance.PushPose(PoseType.Routes);
			Destroy(this.gameObject);
		};

	}
}
