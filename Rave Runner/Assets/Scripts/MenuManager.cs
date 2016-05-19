﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public Animator anim;
    public GameObject[] music;
    public GameObject[] sound;

    bool musicEnabled = true;
    bool soundEnabled = true;

	public void MenuNav (int number)
	{
        anim.SetInteger("MenuNav", number);
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
            music[0].SetActive(true);
            music[1].SetActive(false);
        }
        else
        {
            music[0].SetActive(false);
            music[1].SetActive(true);
        }
    }

    public void Sound()
    {
        soundEnabled = !soundEnabled;

        if (soundEnabled)
        {
            sound[0].SetActive(true);
            sound[1].SetActive(false);
        }
        else
        {
            sound[0].SetActive(false);
            sound[1].SetActive(true);
        }
    }
}