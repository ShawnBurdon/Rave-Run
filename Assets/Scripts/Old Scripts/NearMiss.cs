using UnityEngine;
using System.Collections;

public class NearMiss : MonoBehaviour
{

	public PlayerManager playerManager;

	void OnTriggerExit2D (Collider2D myCollider)
	{
		if (myCollider.tag == "Obstacle")
		{
			playerManager.NearMiss(true);
		}
	}
}
