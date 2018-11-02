using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : InteractiveObject
{
	private void Start()
	{
		OnInteractive += delegate {

			print("打翻水桶");
		};

	}
}
