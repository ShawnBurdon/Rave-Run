using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {

	public float speed;
	public Transform target;
	[HideInInspector]
	public Vector3 laserStartPos;
	[HideInInspector]
	public bool move;
	[HideInInspector]
	public bool moveOffScreen;
	private bool stopMoving;

	void Update ()
	{
		if (move)
		{
			stopMoving = false;

			Vector3 diffMouse = target.position - transform.position;
			diffMouse.Normalize();
			float rot_z = Mathf.Atan2(diffMouse.y, diffMouse.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

			Vector2 moveDirection =(new Vector2(target.position.x, target.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
			GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
			move = false;
		}


		Vector3 xPos = Camera.main.ScreenToWorldPoint(Vector3.zero);
		if (transform.position.x < xPos.x - 1.0f || transform.position.y < xPos.y - 1.0f)
		{
			moveOffScreen = true;
			stopMoving = true;
		}

		if (stopMoving)
			transform.localPosition = laserStartPos;
	}

	void OnTriggerEnter2D (Collider2D myCollider)
	{
		if (myCollider.tag == "Player")
		{
			moveOffScreen = true;
			stopMoving = true;
		}
	}
}
