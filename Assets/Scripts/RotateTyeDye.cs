using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RotateTyeDye : MonoBehaviour
{
	public float speed;

	void Awake ()
	{
	
	}

	void Start ()
	{
	
	}
	
	void Update ()
	{
		transform.Rotate(Vector3.forward * speed);
	}
}