using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public LayerMovement layerMovement;
	public HydrationBar hydrationBar;
    public GameObject canvas;
	public GameObject finishPanel;
	public GameObject player;
	public Animator doubleWaterAnim;
	public Animator extraLifeAnim;
	public Text distanceText;
	public Text gameOverScore;
	public Text gameOverBest;
	public float score;
	public bool extraLifeActive;
	public SpriteRenderer[] doubleWaterRens;
	public Color doubleWaterColor;
	public Color standardWaterColor;
	public Image waterBar;

	public static bool paused;
	public static bool gameStarted;
	public static bool gameOver;
	public static int currentLevel;
	public static float distance;
	public static bool doubleWaterActive;

	void Awake ()
	{
		PlayerPrefs.SetInt("ExtraLives", 10);
		PlayerPrefs.SetInt("DoubleWater", 10);
	}

	void Start ()
	{
		//AppAdvisory.Ads.AdsManager.Instance.ShowBanner();
	}

	void GameOver ()
	{
		if (PlayerPrefs.GetInt("ExtraLives") > 0 && !extraLifeActive)
			StartCoroutine(ExtraLivesBooster());

		paused = true;
		gameStarted = false;
		player.GetComponent<Animator>().SetBool("Dead", true);
		StartCoroutine("Wait");
	}
	public void Pause ()
	{
        Debug.Log("Here");
		paused = !paused;
	}

	public void StartGame(bool extraLife = false)
	{

		if (player.GetComponent<Animator>().GetBool("Dead") == true)
		{
			player.GetComponent<Animator>().SetBool("Dead", false);
		}

		player.transform.position = player.GetComponent<Player>().startPos;
		player.GetComponent<Animator>().SetBool("GameStarted", true);

		player.GetComponent<Rigidbody2D>().gravityScale = 1;


		currentLevel = 0;
		distance = 0;

		Time.timeScale = 1;

		gameStarted = true;
		gameOver = false;
		paused = false;
		extraLifeActive = false;
		doubleWaterActive = false;

		hydrationBar.StartHydrationBar();
		layerMovement.Restart(true);
		player.GetComponent<Player>().Reset();


		foreach (SpriteRenderer water in doubleWaterRens)
		{
			water.color = standardWaterColor;
		}

		waterBar.color = standardWaterColor;

		if (PlayerPrefs.GetInt("DoubleWater") > 0)
			StartCoroutine(DoubleWaterBooster());

	}

	IEnumerator DoubleWaterBooster ()
	{
		doubleWaterAnim.SetBool("IsActive", true);
		yield return new WaitForSeconds(3f);
		doubleWaterAnim.SetBool("IsActive", false);
	}

	IEnumerator ExtraLivesBooster()
	{
		extraLifeAnim.SetBool("IsActive", true);
		yield return new WaitForSeconds(3f);
		extraLifeAnim.SetBool("IsActive", false);
	}

	public void DoubleWater ()
	{
		if (!doubleWaterActive)
		{
			doubleWaterActive = true;
			doubleWaterAnim.SetBool("IsActive", false);

			foreach(SpriteRenderer water in doubleWaterRens)
			{
				water.color = doubleWaterColor;
			}
			waterBar.color = doubleWaterColor;

			int temp = PlayerPrefs.GetInt("DoubleWater");
			PlayerPrefs.SetInt("DoubleWater", temp - 1);
			PlayerPrefs.Save();
		}
    }

	public void ExtraLife()
	{
		if (!extraLifeActive)
		{
			StopCoroutine("Wait");

			extraLifeActive = true;

			extraLifeAnim.SetBool("IsActive", false);
			player.GetComponent<Animator>().SetBool("Dead", false);

			int temp = PlayerPrefs.GetInt("ExtraLives");
			PlayerPrefs.SetInt("ExtraLives", temp - 1);
			PlayerPrefs.Save();

			gameStarted = true;
			gameOver = false;
			paused = false;

			hydrationBar.StartHydrationBar();
			//layerMovement.RestartOnLevel();
		}
	}

	IEnumerator Wait ()
	{
		if (PlayerPrefs.GetInt("ExtraLives") > 0)
			yield return new WaitForSeconds(3f);
		else
			yield return new WaitForSeconds(1f);

        canvas.GetComponent<Animator>().SetTrigger("End");
		
		gameOverScore.text = (int)score + "m";


        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            gameOverBest.text = (int)score + "m";
            PlayerPrefs.SetInt("Highscore", (int)score);
        }
        else
            gameOverBest.text = PlayerPrefs.GetInt("Highscore").ToString();

		PlayerPrefs.Save();

		paused = false;

		AppAdvisory.Ads.AdsManager.Instance.ShowInterstitial();
	}

	public void PauseQuit ()
	{
		canvas.GetComponent<Animator>().SetTrigger("End");

		gameOverScore.text = (int)distance + "m";


		if (score > PlayerPrefs.GetInt("Highscore"))
		{
			gameOverBest.text = (int)score + "m";
			PlayerPrefs.SetInt("Highscore", (int)score);
		}
		else
			gameOverBest.text = PlayerPrefs.GetInt("Highscore").ToString() + "m";


		PlayerPrefs.Save();
		paused = false;
		gameStarted = false;

	}

	void Update ()
	{
		if (!paused && gameStarted)
		{
			if (gameOver)
				GameOver();
			
			distance += 0.02f * layerMovement.speedMultiplier;

			if (doubleWaterActive)
				score = distance * 2;
			else
				score = distance;
			
			distanceText.text = (int)score + "m";
		}

	}
}