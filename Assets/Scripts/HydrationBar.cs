using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HydrationBar : MonoBehaviour
{
	public Image waterBar;
	public AudioSource audioSource;

    float lerpTime;
    float currentLerpTime;

	public void StartHydrationBar ()
	{
		StopAllCoroutines();
		waterBar.fillAmount = 1.0f;
		//StartCoroutine(WaterDepletion());
	}

	void Update ()
	{
		if (GameManager.gameStarted && !GameManager.paused)
		{
			if (waterBar.fillAmount <= 0f)
			{
				//GameManager.gameOver = true;//REMOVE FOR DEATH
			}

			if (!GameManager.doubleWaterActive)
				waterBar.fillAmount -= 0.01f / 1f * Time.deltaTime;
			else
				waterBar.fillAmount -= 0.005f / 1f * Time.deltaTime;
		}
	}

 //   IEnumerator WaterDepletion ()
	//{
//		if (waterBar.fillAmount <= 0.5f)
//		{
//			if (player.position.y > -6.0f)
//				waterBar.fillAmount -= 0.005f;
//			else
//			    waterBar.fillAmount -= 0.01f;
//		}
//		else
//		{
//			if (player.position.y > -6.0f)
//			{
//				float x = waterBar.fillAmount - 0.50f;
//				x = x/100;
//				x = 0.005f - x;
//				waterBar.fillAmount -= x;
//			}
//			else
//			{
//				float x = waterBar.fillAmount - 0.50f;
//				x = x/50;
//				x = 0.005f - x;
//				waterBar.fillAmount -= x;
//			}
//		}
		//waterBar.fillAmount -= 0.01f;

		//yield return new WaitForSeconds(1f);
		//StartCoroutine(WaterDepletion());
	//}

	public void Paused ()
	{
		//StopAllCoroutines();
	}

	public void NotPaused ()
	{
		//StartCoroutine(WaterDepletion());
	}

	public void AddWater ()
	{
		waterBar.fillAmount += 0.01f;
	}

	public void LoseWater ()
	{
		if (!GameManager.doubleWaterActive)
			waterBar.fillAmount -= 0.33f;
		else
			waterBar.fillAmount -= 0.165f;

	}
}
