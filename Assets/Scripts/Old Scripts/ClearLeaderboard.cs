using UnityEngine;
using System.Collections;

public class ClearLeaderboard : MonoBehaviour {

	public TextMesh hangarDistanceText;                     //A link to the hangar distance text
	public TextMesh clickToReset;
	public static bool clearing;

	private bool wait;
	
	void OnMouseDown ()
	{
		clearing = true;
		PlayerPrefs.SetInt("Best Distance", 0);
		SaveManager.bestDistance = 0;
		hangarDistanceText.text = SaveManager.bestDistance + " M";
	}

	void OnMouseUp ()
	{
		StartCoroutine(Wait());
	}

	IEnumerator Wait ()
	{
		yield return new WaitForSeconds(0.1f);
		clearing = false;
		gameObject.SetActive(false);
	}
}