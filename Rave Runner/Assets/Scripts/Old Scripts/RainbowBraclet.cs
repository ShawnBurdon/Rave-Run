using UnityEngine;
using System.Collections;

public class RainbowBraclet : MonoBehaviour
{

	private bool move;
	private GameObject player;
	private float timeStartedLerping;
	private float timeSinceStarted;
	private float timeTakenDuringLerp = 1f;
	private Vector3 startPos;

	void Start ()
	{
		startPos = transform.position;
	}

	void OnEnable ()
	{	
		if (move)
		{
			move = false;
			transform.position = startPos;
		}
	}

	void FixedUpdate ()
	{
		if (move)
		{
			float timeSinceStarted = Time.time - timeStartedLerping;
			float percentageComplete = (timeSinceStarted / timeTakenDuringLerp)*0.65f;

			transform.position = Vector3.Lerp(transform.position, player.transform.position, percentageComplete);
		}
	}

	void OnTriggerEnter2D(Collider2D myCollider)
	{
		//If the submarine is collided with a coin
		if (myCollider.gameObject.name == "RainbowBracelet")
		{
			timeStartedLerping = Time.time;
			player = myCollider.gameObject;
			move = true;
		}
	}
}
