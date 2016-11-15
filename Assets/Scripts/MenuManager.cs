using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public Animator anim;
    public GameObject[] music;
    public GameObject[] sound;
	public AudioSource musicSource;
	public AudioSource soundSource;
	public AudioClip[] menuMusic;

    bool musicEnabled = true;
    bool soundEnabled = true;

	public void MenuNav (int number)
	{
        anim.SetInteger("MenuNav", number);
	}

	public void Quit ()
	{
		Application.Quit();
	}

	public void StoreNav (int number)
    {
        anim.SetInteger("StoreNav", number);
    }

    public void PauseBackButton ()
    {
        if (!GameManager.paused)
            anim.SetInteger("MenuNav", 0);
        else
            anim.SetTrigger("Back");
    }

    public void PauseBool (bool paused)
    {
        anim.SetBool("Pause", paused);
    }

    public void Music ()
    {
        musicEnabled = !musicEnabled;

        if (musicEnabled)
        {
			musicSource.enabled = true;

            music[0].SetActive(true);
            music[1].SetActive(false);
			musicSource.Play();
        }
        else
        {
			musicSource.enabled = false;

			music[0].SetActive(false);
            music[1].SetActive(true);
        }
    }

    public void Sound()
    {
        soundEnabled = !soundEnabled;

        if (soundEnabled)
        {
			soundSource.enabled = true;

			sound[0].SetActive(true);
            sound[1].SetActive(false);
        }
        else
        {
			soundSource.enabled = false;

			sound[0].SetActive(false);
            sound[1].SetActive(true);
        }
    }

	void Start ()
	{
		StartMenuMusic();
	}

	public void StopMenuMusic()
	{
		StopCoroutine("PlaySong");
	}

	public void StartMenuMusic()
	{
		StartCoroutine("PlaySong");
	}

	IEnumerator PlaySong()
	{
		for (float i = 40; i > 0; i--)
		{
			musicSource.volume = i / 40f;
			yield return new WaitForSeconds(0.001f);
		}

		musicSource.clip = menuMusic[0];
		musicSource.Play();

		for (float i = 0; i < 40; i++)
		{
			musicSource.volume = i / 40f;
			yield return new WaitForSeconds(0.001f);
		}

		yield return new WaitForSeconds(menuMusic[0].length - 0.04f);
		musicSource.clip = menuMusic[1];
		musicSource.Play();
		musicSource.loop = true;
	}
}