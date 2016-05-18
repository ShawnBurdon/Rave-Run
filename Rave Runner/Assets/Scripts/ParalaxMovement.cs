using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ParalaxMovement : MonoBehaviour
{
	public LayerMovement layerMovement;
	public float startingSpeed;
	public float xDistance;
	public int amountActive;
	public float destroyXSpot;
	public float startXSpot;

	private List<Transform> activeElements;   //Contains the active layer elements, which has not reached the spawnAt position
	private List<Transform> inactive;         //Contains the inactive layer elements
	private bool removeLast;
	private bool changingLevels;

	void OnEnable ()
	{
		ActivateLevel();
	}

	void ActivateLevel ()
	{

		activeElements = new List<Transform>();
		inactive = new List<Transform>();
		

		foreach (Transform child in transform)
			inactive.Add(child);

		foreach (Transform child in transform)
			child.gameObject.SetActive(false);

		//Spawn the element
		for (int x=0; x<amountActive; x++)
		{
				SpawnElement();
		}
	}
	
	void Update ()
	{
		if (!GameManager.paused && GameManager.gameStarted)
		{
			//Loop through the active elemets
			foreach (Transform item in activeElements)
			{
				//Move the item to the left
				item.transform.position -= Vector3.right * startingSpeed * layerMovement.speedMultiplier * Time.deltaTime;
				
				//If the item has reached the reset position
				if (item.transform.position.x < destroyXSpot)
					removeLast = true;
			}
			
			if (removeLast)
			{
				removeLast = false;
				RemoveElement(activeElements[0]);
			}
		}
	}

	void SpawnElement(bool starting = false)
	{	
		Transform item;
		float xpos = 0f;
		
		if (starting)
			item = inactive[0];
		else
			item = inactive[Random.Range(0, inactive.Count)];
		
		if (activeElements.Count > 0)
		{
			xpos = activeElements[activeElements.Count-1].position.x + xDistance;
		}
		else
		{
			xpos = startXSpot;
		}
		
		foreach (Transform child in item)
		{
			child.gameObject.SetActive(true);
		}
		
		//Activate it, and add it to the active elements
		item.gameObject.SetActive(true);
		inactive.Remove(item);
		activeElements.Add(item);
		
		item.transform.position = new Vector3(xpos, item.transform.position.y, 0);
	}

	//Removes the first element
	void RemoveElement(Transform item)
	{
		//Remove it from the active elements, add it to the inactive, and disable it
		activeElements.Remove(item);
		inactive.Add(item);
		
		item.gameObject.SetActive(false);

		SpawnElement();
	}
}