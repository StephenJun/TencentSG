using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : InteractiveObject {

	private int showTime;

	private void Start()
	{
		showTime = Random.Range(0,24);
		LevelController.Instance.SmartDoorHandler(showTime);
		OnInteractive += delegate
		{
			showTime = Random.Range(0, 24);
			LevelController.Instance.SmartDoorHandler(showTime);
			detailInfo = "Current time is " + showTime.ToString();
		};
	}
}
