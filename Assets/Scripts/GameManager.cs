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
	public Animator doublePointsAnim;
	public Animator extraLifeAnim;
	public Text distanceText;
	public Text gameOverScore;
	public Text gameOverBest;
	public float score;
	public bool doublePointsActive;
	public bool extraLifeActive;

	public static bool paused;
	public static bool gameStarted;
	public static bool gameOver;
	public static int currentLevel;
	public static float distance;
		
	void Awake ()
	{

	}

	void GameOver ()
	{
		if (PlayerPrefs.GetInt("ExtraLives") > 0)
			StartCoroutine(ExtraLivesBooster());

		paused = true;
		gameStarted = false;
		player.GetComponent<Animator>().SetBool ("Dead", true);
		//		player.GetComponent<Animator>().SetBool("GameOver", true);
		StartCoroutine("Wait");
	}
	public void Pause ()
	{
        Debug.Log("Here");
		paused = !paused;
	}

	public void StartGame()
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

		gameStarted = true;
		gameOver = false;
		paused = false;
		doublePointsActive = false;
		extraLifeActive = false;

		hydrationBar.StartHydrationBar();
		layerMovement.Restart(true);

		//if (PlayerPrefs.GetInt("DoublePoints") > 0)
		//	StartCoroutine(DoublePointsBooster());

	}

	IEnumerator DoublePointsBooster ()
	{
		doublePointsAnim.SetBool("IsActive", true);
		yield return new WaitForSeconds(3f);
		doublePointsAnim.SetBool("IsActive", false);
	}

	IEnumerator ExtraLivesBooster()
	{
		extraLifeAnim.SetBool("IsActive", true);
		yield return new WaitForSeconds(3f);
		extraLifeAnim.SetBool("IsActive", false);
	}

	public void DoublePoints ()
	{
		if (!doublePointsActive)
		{
			doublePointsActive = true;
			doublePointsAnim.SetBool("IsActive", false);

			int temp = PlayerPrefs.GetInt("DoublePoints");
			PlayerPrefs.SetInt("DoublePoints", temp - 1);
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

		paused = false;
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

			if (doublePointsActive)
				score = distance * 2;
			else
				score = distance;
			
			distanceText.text = (int)score + "m";
		}

	}
}