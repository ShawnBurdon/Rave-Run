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
	public Text distanceText;
	public Text gameOverScore;
	public Text gameOverBest;

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
		paused = true;
		gameStarted = false;
		player.GetComponent<Animator>().SetBool ("Dead", true);
//		player.GetComponent<Animator>().SetBool("GameOver", true);
		StartCoroutine(Wait ());

	}
	public void Pause ()
	{
        Debug.Log("Here");
		paused = !paused;
	}

	public void StartGame ()
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
		
		hydrationBar.StartHydrationBar();
		layerMovement.Restart(true);
	}


    IEnumerator Wait ()
	{
		yield return new WaitForSeconds(1f);

        canvas.GetComponent<Animator>().SetTrigger("End");
		
		gameOverScore.text = (int)distance + "m";


        if (distance > PlayerPrefs.GetInt("Highscore"))
        {
            gameOverBest.text = (int)distance + "m";
            PlayerPrefs.SetInt("Highscore", (int)distance);
        }
        else
            gameOverBest.text = PlayerPrefs.GetInt("Highscore").ToString();

		paused = false;
	}

	public void PauseQuit ()
	{
		canvas.GetComponent<Animator>().SetTrigger("End");

		gameOverScore.text = (int)distance + "m";


		if (distance > PlayerPrefs.GetInt("Highscore"))
		{
			gameOverBest.text = (int)distance + "m";
			PlayerPrefs.SetInt("Highscore", (int)distance);
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

			distanceText.text = (int)distance + "m";
		}

	}
}