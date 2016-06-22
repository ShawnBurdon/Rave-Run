using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayerMovement : MonoBehaviour 
{
	public Transform[] levelContainers;
	public Transform[] endLevelPieces;
	public GameObject[] paralaxContainers;
	public GameObject[] backgrounds;
	public float startingSpeed;                 //The starting speed of the layer
	public float speedMultiplier=1;            //The current speed multiplier
	public float increaseRate;
	public AudioClip[] songs;
	public AudioSource audioPlayer;

	public List<Transform> activeElements;   //Contains the active layer elements, which has not reached the spawnAt position
	private List<Transform> inactive;         //Contains the inactive layer elements
	private Transform container;                 //Contains the layer elements
	private bool removeLast;
	private bool changingLevels;
	private int overallLevel = 1;
	public int currentLev;

	void Start ()
	{
		ActivateLevel();
	}
	// Update is called once per frame
	void Update () 
	{
		currentLev = GameManager.currentLevel;
//************************DEBUG************************
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	foreach (Transform child in container)
		//		child.gameObject.SetActive(false);
			
		//	GameManager.currentLevel++;
		//	ActivateLevel();
		//}
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	foreach (Transform child in container)
		//		child.gameObject.SetActive(false);
			
		//	GameManager.currentLevel--;
		//	ActivateLevel();
		//}
//************************DEBUG************************
		if (overallLevel == 1)
		{
			if (speedMultiplier < 2.0f)
				speedMultiplier += increaseRate;
			else
				speedMultiplier = 2.0f;
		}
		else if (overallLevel == 2)
		{
			if (speedMultiplier < 3.0f)
				speedMultiplier += increaseRate;
			else
				speedMultiplier = 3.0f;
		}
		else if (overallLevel == 3)
		{
			if (speedMultiplier < 4.0f)
				speedMultiplier += increaseRate;
			else
				speedMultiplier = 4.0f;
		}
		else if (overallLevel >= 4)
		{
			if (speedMultiplier < 5.0f)
				speedMultiplier += increaseRate;
			else
				speedMultiplier = 5.0f;
		}

		if (!GameManager.paused && GameManager.gameStarted)
		{
			//Loop through the active elemets
			foreach (Transform item in activeElements)
			{
				//Move the item to the left
				item.transform.position -= Vector3.right * startingSpeed * speedMultiplier * Time.deltaTime;
				
				//If the item has reached the reset position
				if (item.transform.position.x < -32f)
					removeLast = true;
			}

			if (removeLast)
			{
				removeLast = false;
				RemoveElement(activeElements[0]);
			}
			
			if (GameManager.distance >= 650 && GameManager.currentLevel == 0 && !changingLevels)//level 2
			{
				Debug.Log ("Spawn level 2");
				SpawnEndElement();
			}
			else if (GameManager.distance >= 1810 && GameManager.currentLevel == 1 && !changingLevels)//level 3
			{
				Debug.Log ("Spawn level 3");
				SpawnEndElement();
			}
			else if (GameManager.distance >= 3300 && GameManager.currentLevel == 2 && !changingLevels)//level 4
			{
				Debug.Log ("Spawn level 4");
				SpawnEndElement();
			}
		}
	}

	public void RestartOnLevel()
	{
		if (GameManager.currentLevel == 1)
		{
			IncreaseLevel();
		}
		else if (GameManager.currentLevel == 2)
		{
			IncreaseLevel();
			IncreaseLevel();
		}
		else if (GameManager.currentLevel == 3)
		{
			IncreaseLevel();
			IncreaseLevel();
			IncreaseLevel();
		}

		Restart();

		if (GameManager.currentLevel == 1)
		{
			speedMultiplier = 1.0f;
		}
		else if (GameManager.currentLevel == 2)
		{
			speedMultiplier = 2.0f;
		}
		else if (GameManager.currentLevel == 3)
		{
			speedMultiplier = 3.0f;
		}
		else if (GameManager.currentLevel == 4)
		{
			speedMultiplier = 4.0f;
		}
		else if (GameManager.currentLevel >= 5)
		{
			speedMultiplier = 5.0f;
		}
	}

	public void SpawnEndElement()
	{	
		changingLevels = true;

		Transform item = endLevelPieces[GameManager.currentLevel];
		float xpos = 0f;

		Debug.Log ("Spawning end element in spot " + GameManager.currentLevel);

		if (activeElements [activeElements.Count - 1].tag == "Small")
			xpos = activeElements [activeElements.Count - 1].position.x + 20.48f;
		else if (activeElements[activeElements.Count-1].tag == "Large")
			xpos = activeElements[activeElements.Count-1].position.x + 25.60f;

		//Activate it, and add it to the active elements
		item.gameObject.SetActive(true);
		activeElements.Add(item);
		
		item.transform.position = new Vector3(xpos, item.transform.position.y, 0);
	}

	public void IncreaseLevel ()
	{
		foreach (Transform child in container)
			child.gameObject.SetActive(false);

		endLevelPieces[GameManager.currentLevel].gameObject.SetActive(false);

		if (GameManager.currentLevel < 3)
			GameManager.currentLevel++;
		else 
			GameManager.currentLevel = 0;

		overallLevel++;
		
		ActivateLevel();
	}

	IEnumerator FadeMusic()
	{
		for (int i = 100; i > 0; i--)
		{
			audioPlayer.volume = i/100;
			yield return new WaitForSeconds(0.015f);
		}
		
		audioPlayer.clip = songs[GameManager.currentLevel];
		audioPlayer.Play();

		for (int i = 0; i < 100; i++)
		{
			audioPlayer.volume = i/100;
			yield return new WaitForSeconds(0.015f);
		}
	}

	public void Restart (bool start = false) 
	{   
		if (start)
			speedMultiplier = 1;
	
		changingLevels = false;

		if(container)
		{
			foreach (Transform child in container)
				child.gameObject.SetActive(false);
		}

		foreach (Transform child in container)
			inactive.Add(child);

		ActivateLevel();

		audioPlayer.clip = songs[GameManager.currentLevel];
		audioPlayer.Play();
	}

	void ActivateLevel ()
	{
		changingLevels = false;

		activeElements = new List<Transform>();
		inactive = new List<Transform>();

		container = levelContainers[GameManager.currentLevel];

		foreach (Transform child in container)
			inactive.Add(child);

		foreach (GameObject child in paralaxContainers)
			child.SetActive(false);

		foreach (GameObject back in backgrounds)
			back.SetActive(false);
	
		paralaxContainers[GameManager.currentLevel].SetActive(true);
		backgrounds[GameManager.currentLevel].SetActive(true);


		//Spawn the element
		for (int x=0; x<3; x++)
		{
			if (x<2)
				SpawnElement(true);
//			else if (speedMultiplier > 1.5f && x<5)
//				SpawnElement(true);
			else
				SpawnElement();
		}
	}

	//Spawns a new layer element
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
			if (activeElements[activeElements.Count-1].tag == "Large" && item.tag == "Large")
				xpos = activeElements[activeElements.Count-1].position.x + 46.08f;
			else if (activeElements[activeElements.Count-1].tag == "Small" && item.tag == "Large")
				xpos = activeElements[activeElements.Count-1].position.x + 40.96f;
			else if (activeElements[activeElements.Count-1].tag == "Large" && item.tag == "Small")
				xpos = activeElements[activeElements.Count-1].position.x + 40.96f;
			else if (activeElements[activeElements.Count-1].tag == "Small" && item.tag == "Small")
				xpos = activeElements[activeElements.Count-1].position.x + 30.72f;
		}
		else
		{
			xpos = 0f;
		}

		foreach (Transform child in item)
		{
			child.gameObject.SetActive(true);
		}

		//Activate it, and add it to the active elements
		item.gameObject.SetActive(true);
		inactive.Remove(item);
		activeElements.Add(item);
//		if (GameManager.currentLevel == 3)
//		{
//			float yPos = Random.Range(
//		}
//		else
			item.transform.position = new Vector3(xpos, item.transform.position.y, 0);
	}

	//Removes the first element
	void RemoveElement(Transform item)
	{
		//Remove it from the active elements, add it to the inactive, and disable it
		activeElements.Remove(item);
		inactive.Add(item);
		
		item.gameObject.SetActive(false);

		if (!changingLevels)
			SpawnElement();
	}
}