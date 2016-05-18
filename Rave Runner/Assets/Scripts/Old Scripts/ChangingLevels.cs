using UnityEngine;
using System.Collections;

public class ChangingLevels : MonoBehaviour
{
	public GameObject player;
	
	private bool grounded;
	private bool fallingToSub;
	private Camera cam;
	private Vector3 startingPos;

	//Needs to move -6.75
	void Start ()
	{
		cam = Camera.main;
		startingPos = cam.transform.position;
	}
	
	void Update ()
	{
		if (player.transform.position.y > 0.0f)
		{
			fallingToSub = false;
		}

		if (grounded && !fallingToSub && player.transform.position.y < 0.0f)
			cam.transform.position = startingPos;


		if (!grounded && !PlayerManager.gamePaused)
		{
			//Move Camera
			cam.transform.position = new Vector3 (0.0f, player.transform.position.y + 2.755f, cam.transform.position.z);
		
			//clamp camera if its at ground level
			if (cam.transform.position.y < 0.0f && !fallingToSub)
			{
				cam.transform.position = new Vector3 (0.0f, 0.0f, cam.transform.position.z);
			}

			else if (cam.transform.position.y > 15.35f)
			{
				cam.transform.position = new Vector3 (0.0f, 15.35f, cam.transform.position.z);
			}
			else if (cam.transform.position.y < 0 && grounded)
			{
				cam.transform.position = new Vector3 (0.0f, -6.64f, cam.transform.position.z);

			}
		}
				
		cam.transform.position = new Vector3 (0.0f, Mathf.Clamp(cam.transform.position.y, -6.64f, 15.35f), cam.transform.position.z);
	}
	public void SwitchingLevels ()
	{
		grounded = false;
	}
	
	public void Grounded ()
	{
		grounded = true;
	}

	public void SubGround ()
	{
		fallingToSub = true;
	}

	public void Reset ()
	{
		cam.transform.position = startingPos;
	}
}