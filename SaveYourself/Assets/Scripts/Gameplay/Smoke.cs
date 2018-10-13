﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class Smoke : MonoBehaviour {

	public float maxDamagePerSecond = 10;

	private SphereCollider col;
	private float maxRange;

	private void Start()
	{
		col = GetComponent<SphereCollider>();
		maxRange = col.radius * transform.localScale.x;
	}

	private void OnTriggerStay(Collider other)
	{
		PlayerController pc = other.GetComponent<PlayerController>();
		if (pc)
		{
			float dis = Vector3.Distance(other.transform.position, transform.position);
			pc.DamageReceiver(maxDamagePerSecond * Time.deltaTime * (1- dis/maxRange));
		}
	}
}
