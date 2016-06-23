using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public LaserBeam laserBeam;
	public GameObject target;

	private Vector3  playerPos;
	private Vector3 startPos;
	private Vector3 stopPos;
	private bool stopMoving;

	void Start	()
	{
		startPos = transform.localPosition;
	}
	void Update ()
	{
		if (stopMoving)
			transform.position = stopPos;
		if (laserBeam.moveOffScreen)
		{
			stopMoving = false;
			transform.Translate(Vector3.left * Time.deltaTime * 4.5f); 
		}
	
		Vector3 xPos = Camera.main.ScreenToWorldPoint(Vector3.zero);
		if (transform.position.x < xPos.x - 3.0f)
		{
			transform.position = Vector3.zero;
			Reset();
		}

	}

	void OnTriggerEnter2D (Collider2D myCollider)
	{
		if (myCollider.gameObject.tag == "Player" && !laserBeam.moveOffScreen)
		{
			//stop movement
			stopMoving = true;
			stopPos = transform.position;
			laserBeam.laserStartPos = laserBeam.transform.localPosition;
			//wait a second
			StartCoroutine(Wait());
			//shoot from coroutine
		}
	}

	void OnTriggerStay2D (Collider2D myCollider)
	{
		playerPos = myCollider.transform.position;
	}

	IEnumerator Wait ()
	{
		yield return new WaitForSeconds(0.5f);
		laserBeam.move = true;
		target.transform.position = new Vector3(playerPos.x, playerPos.y + 1, playerPos.z);
	}

	public void Reset ()
	{
		transform.localPosition = startPos;
		laserBeam.move = false;
		laserBeam.moveOffScreen = false;
	}

}