﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Neuroi : MonoBehaviour
{
	//big neuroi and small neuroi classes will inherit this abstract class
	public static float Speed { get; set; }
	private int score;

	public virtual void Shoot()
	{
		Debug.Log("Laser");
	}
}