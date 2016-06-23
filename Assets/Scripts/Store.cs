using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Store : MonoBehaviour
{
	
	void Awake ()
	{
	
	}

	public void DoublePointsBooster ()
	{
		PlayerPrefs.SetInt("DoublePoints", PlayerPrefs.GetInt("DoublePoints") + 1);
	}
	
	public void ExtraLifeBooster ()
	{
		PlayerPrefs.SetInt("ExtraLives", PlayerPrefs.GetInt("ExtraLives") + 1);
	}
}