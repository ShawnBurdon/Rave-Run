using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public LevelManager levelManager;                       //A link to the level manager
    public PlayerManager playerManager;                     //A link to the player manager
    public PowerupManager powerupManager;                   //A link to the powerup manager

    public TextMesh hangarDistanceText;                     //A link to the hangar distance text

    public Text distanceText;                               //The main UI's distance text
    public Text coinText;                                   //The main UI's coin ammount text

    public Text finishDistanceText;                         //The finish menu's distance text
    public Text finishCoinText;                             //The finish menu's coin ammount text

    public GameObject mainUI;                               //The main UI

    public Sprite[] arrowSprites;                           //Up and down arrow sprites
    public Sprite[] audioSprites;                           //Audio enabled and disabled sprites
    public Sprite[] ShopSkinButtonSprites;                  //The buy, equip, equipped sprites

    public Animator overlayAnimator;                        //The overlay animator
    public Animator topMenuAnimator;                        //The top menu animator
    public Animator shopAnimator;                           //The shop menu animator
    public Animator pauseMenuAnimator;                      //The pause menu animator
    public Animator finishMenuAnimator;                     //The finish menu animator
    public Animator[] powerupButtons;                       //The powerup buttons animator (extra speed, shield, sonic wave, revive)

    public RectTransform[] missionPanelElements;            //The mission panel elements (mission descriptions and mission status)

    public Text[] ShopOwnedItems;                           //The shop menu number of owned item texts

    public Image[] shopSubmarineButtons;                    //The shop menu submarine 1 and 2 buttons
    public Image[] audioButtons;                            //The audio buttons

    //Tells, which mission notification is used at the moment
    private bool[] usedMissionNotifications = new bool[]{false, false, false};

    public static bool inPlayMode = false;

    private int distanceTraveled = 0;

    //Called at the beginning of the game
    void Start()
    {
        //Updates the audio buttons sprites
        UpdateAudioButtons();

    }
    //Called at every frame
    void Update()
    {
        //If the game is in play mode
        if (inPlayMode)
        {
            //Update the main UI's status texts
            distanceText.text = AddDigitDisplay(distanceTraveled, 5);
        }
    }
    //Called, when the player clicks on the top menu arrow button
    public void ChangeTopMenuState(Image arrowImage)
    {
        //If the top menu is in the default position
        if (!topMenuAnimator.GetBool("MoveDown"))
        {
            //Change the button sprite, and move the menu down
            arrowImage.sprite = arrowSprites[1];
            topMenuAnimator.SetBool("MoveDown", true);
            overlayAnimator.SetBool("Visible", true);
        }
        else
        {
       
        }
    }
    //Called, when the player clicks on an audio button. Change audio state (enabled, disabled)
    public void ChangeAudioState()
    {
        //Change the state, and update the button sprites
        AudioManager.Instance.ChangeAudioState();
        UpdateAudioButtons();
    }
    //Called when the player click on a shop button
    public void ToggleShopMenu()
    {
        //Make sure that the shop is activated
        shopAnimator.gameObject.SetActive(true);

        //If the shop panel is visible
        if (shopAnimator.GetBool("ShowPanel"))
        {
            //Hide and disable it
            shopAnimator.SetBool("ShowPanel", false);
            StartCoroutine(DisableMenu(shopAnimator, 0.5f));
        }
        //If the shop menu is hidden
        else
        {
            //Update the shop, and move it to the view
            UpdateShopDisplay();
            shopAnimator.SetBool("ShowPanel", true);
        }
    }
    //Called when the player click on the top menu's mission button
   
    //Called, when the player buys an extra speed powerup
    public void BuySpeed(Text priceTag)
    {
        //Obtain price from the pricetag text
        int price = int.Parse(priceTag.text);

        //If the player can purchase the powerup
        if (SaveManager.coinAmmount - price >= 0)
        {
            //Decrease coin ammount, and increase powerup count
            SaveManager.coinAmmount -= price;
            SaveManager.extraSpeed += 100000;
            SaveManager.SaveData();

        }
    }
    //Called, when the player buys a shield powerup
    public void BuyShield(Text priceTag)
    {
        //Obtain price from the pricetag text
        int price = int.Parse(priceTag.text);

        //If the player can purchase the powerup
        if (SaveManager.coinAmmount - price >= 0)
        {
            //Decrease coin ammount, and increase powerup count
            SaveManager.coinAmmount -= price;
            SaveManager.shield += 1;
            SaveManager.SaveData();

            //Notify mission manager, and update shop display
            UpdateShopDisplay();
        }
    }
    //Called, when the player buys a sonic blast powerup
    public void BuySonicBlast(Text priceTag)
    {
        //Obtain price from the pricetag text
        int price = int.Parse(priceTag.text);

        //If the player can purchase the powerup
        if (SaveManager.coinAmmount - price >= 0)
        {
            //Decrease coin ammount, and increase powerup count
            SaveManager.coinAmmount -= price;
            SaveManager.sonicWave += 1;
            SaveManager.SaveData();

            //Notify mission manager, and update shop display
            UpdateShopDisplay();
        }
    }
    //Called, when the player buys a revive powerup
    public void BuyRevive(Text priceTag)
    {
        //Obtain price from the pricetag text
        int price = int.Parse(priceTag.text);

        //If the player can purchase the powerup
        if (SaveManager.coinAmmount - price >= 0)
        {
            //Decrease coin ammount, and increase powerup count
            SaveManager.coinAmmount -= price;
            SaveManager.revive += 10000;
            SaveManager.SaveData();

            //Notify mission manager, and update shop display

        }
    }
	public void BuyRainbowBracelet(Text priceTag)
	{
		//If the Rz is not yet owned
		if (SaveManager.rainbowBraceletUnlocked == 0)
		{
			//Obtain the price from the pricetag text
			int rbPrice = int.Parse(priceTag.text);
			
			//If the player can purchase the submarine
			if (SaveManager.coinAmmount - rbPrice >= 0)
			{
				//Decrease coin ammount, and unlock the green submarine
				SaveManager.coinAmmount -= rbPrice;
				SaveManager.rainbowBraceletUnlocked = 1;
				SaveManager.SaveData();
				
				////Update the player, and the shop display
				UpdateShopDisplay();
			}
		}
	}

    //Called, when the player buys the yellow submarine
    public void BuySubmarine1()
    {
        //Change the current skin ID
        SaveManager.currentSkinID = 0;
        SaveManager.SaveData();

        //Update the player, and the shop display
        playerManager.ChangeSkin(0);
        UpdateShopDisplay();
    }
    //Called, when the player buys the green submarine
    public void BuySubmarine2(Text priceTag)
    {
        //If the submarine is not yet owned
        if (SaveManager.skin2Unlocked == 0)
        {
            //Obtain the price from the pricetag text
            int skin2Price = int.Parse(priceTag.text);

            //If the player can purchase the submarine
            if (SaveManager.coinAmmount - skin2Price >= 0)
            {
                //Decrease coin ammount, and unlock the green submarine
                SaveManager.coinAmmount -= skin2Price;
                SaveManager.skin2Unlocked = 1;
                SaveManager.currentSkinID = 1;
                SaveManager.SaveData();

                ////Update the player, and the shop display
                playerManager.ChangeSkin(1);
                UpdateShopDisplay();
            }
        }
        //If the player already owns the submarine
        else if (SaveManager.currentSkinID != 1)
        {
            //Change the current skin ID
            SaveManager.currentSkinID = 1;
            SaveManager.SaveData();

            //Update the player, and the shop display
            playerManager.ChangeSkin(1);
            UpdateShopDisplay();
        }
    }
    //Called, when the player click on the PlayTrigger
    public void PlayTrigger(Image arrowImage)
    {
        //If the game is not in play mode
        if (!inPlayMode && !ClearLeaderboard.clearing)
        {
            //Set the game to play mode
            inPlayMode = true;
            mainUI.SetActive(true);

			//Starts timer till Bass Cannon dissapears


            //Hide the main menu
            arrowImage.sprite = arrowSprites[0];
            topMenuAnimator.SetBool("Hide", true);
            overlayAnimator.SetBool("Visible", false);

            //Start the level
            levelManager.StartLevel();

            //Show the available powerups
			int[] powerupCount = new int[]{SaveManager.extraSpeed, SaveManager.shield, SaveManager.sonicWave};

            for (int i = 0; i < powerupCount.Length; i++)
                if (powerupCount[i] > 0)
                    powerupButtons[i].SetBool("Visible", true);
			StartCoroutine(BassCannon());
            StartCoroutine(DisableMenu(topMenuAnimator, 1));
        }
    }
    //Called, when the player clicks on the pause button
    public void PauseButton()
    {
        pauseMenuAnimator.gameObject.SetActive(true);

        //If the game is paused
        if (pauseMenuAnimator.GetBool("Visible") == true)
        {
			//Time.timeScale = 1.0f;
            //Hide the pause menu, and activate the main UI
            overlayAnimator.SetBool("Visible", false);
            pauseMenuAnimator.SetBool("Visible", false);
            mainUI.gameObject.SetActive(true);

            //Show the available powerups
            int[] powerupCount = new int[] {SaveManager.shield, SaveManager.sonicWave};

            for (int i = 0; i < powerupCount.Length; i++)
                if (powerupCount[i] > 0)
                    powerupButtons[i].SetBool("Visible", true);

            //Resume the game
            levelManager.ResumeLevel();
            StartCoroutine(DisableMenu(pauseMenuAnimator, 1));
        }
        //If the game is not paused, and can be paused
        else if (powerupManager.CanUsePowerup())
        {
            // Show the pause menu, and disable the main UI
            overlayAnimator.SetBool("Visible", true);
            pauseMenuAnimator.SetBool("Visible", true);
            mainUI.gameObject.SetActive(false);

            //Pause the game
            levelManager.PauseLevel();
			//Time.timeScale = 0.0f;
        }
    }
    //Called, when the player clicks on a retry button
    public void Retry()
    {
		Time.timeScale = 1.0f;
        //Hide the menus
        overlayAnimator.SetBool("Visible", false);

        if (pauseMenuAnimator.gameObject.activeSelf)
        {
            pauseMenuAnimator.SetBool("Visible", false);
            StartCoroutine(DisableMenu(pauseMenuAnimator, 1));
        }

        if (finishMenuAnimator.gameObject.activeSelf)
        {
            finishMenuAnimator.SetBool("Visible", false);
            StartCoroutine(DisableMenu(finishMenuAnimator, 1));
        }
        //Reset the game
        powerupManager.Reset();
        levelManager.Restart();

        //Activate the main UI
        mainUI.gameObject.SetActive(true);

        //Show available powerup
        int[] powerupCount = new int[] {SaveManager.extraSpeed, SaveManager.shield, SaveManager.sonicWave};

        for (int i = 1; i < powerupCount.Length; i++)
            if (powerupCount[i] > 1)
                powerupButtons[i].SetBool("Visible", true);
			
		StartCoroutine(BassCannon());

    }
    //Called, when the player clicks on a quit button
    public void QuitToMain()
    {

        //Disable menus
        overlayAnimator.SetBool("Visible", false);

        if (pauseMenuAnimator.gameObject.activeSelf)
        {
            pauseMenuAnimator.SetBool("Visible", false);
            StartCoroutine(DisableMenu(pauseMenuAnimator, 1));
        }

        if (finishMenuAnimator.gameObject.activeSelf)
        {
            finishMenuAnimator.SetBool("Visible", false);
            StartCoroutine(DisableMenu(finishMenuAnimator, 1));
        }
        
        //Show top menu
        topMenuAnimator.gameObject.SetActive(true);
        topMenuAnimator.SetBool("MoveDown", false);
        topMenuAnimator.SetBool("Hide", false);

        //Reset the coin ammount

        //Reset powerups, and quit to main menu
		powerupManager.Reset();
        levelManager.QuitToMain();
        inPlayMode = false;
		Time.timeScale = 1.0f;
		Application.LoadLevel(0);

    }
    //Receive current distance
    public void UpdateDistance(int newDist)
    {
        distanceTraveled = newDist;
    }
    //Receive collected coins ammount
    public void UpdateCoins(int newCoins)
    {
    }
    //Called, when the player collider with a powerup
    public void ShowPowerup(string name)
    {
        //Increase powerup count, and show powerup icon based on the name of the powerup
        switch (name)
        {
            case "ExtraSpeed":
                SaveManager.extraSpeed += 1;
                break;

            case "Shield":
                SaveManager.shield += 1;
                powerupButtons[1].SetBool("Visible", true);
                break;

            case "SonicBlast":
                SaveManager.sonicWave += 1;
                powerupButtons[2].SetBool("Visible", true);
                break;

            case "Revive":
                SaveManager.revive += 1;
                break;
        }
    }
    //Called, when the player activates a powerup
    public void HidePowerup(Animator anim)
    {
        //If a powerup can't be activated, return
        if (!powerupManager.CanUsePowerup())
            return;

        //Play powerup sound
        AudioManager.Instance.PlayPowerupUsed();

        //Remove a powerup, and activate it's effect, based on it's name
        switch (anim.gameObject.name)
        {
            case "Speed Button":
                SaveManager.extraSpeed -= 1;
                powerupManager.ExtraSpeed();
                break;

            case "Shield Button":
                SaveManager.shield -= 1;
//                powerupManager.Shield();
                break;

            case "Sonic Wave Button":
                SaveManager.sonicWave -= 1;
                powerupManager.SonicBlast();
                break;
        }

        //Save changes, and hide the powerup button
        SaveManager.SaveData();
        anim.SetBool("Visible", false);
    }
    //Called, when the player activates the revive powerup
    public void RevivePlayer()
    {
        //Remove the used revive
        SaveManager.revive -= 1;
        SaveManager.SaveData();

        //Revive the player
        AudioManager.Instance.PlayRevive();
        powerupManager.Revive();
        levelManager.ReviveUsed();
        StopCoroutine("Revive");

        //Hide revive button
        powerupButtons[3].SetBool("Visible", false);
    }
    //Called, after the player has crashed
    public void ShowCrashScreen(int distance)
    {
        //If the player has a revive, show it
        if (SaveManager.revive > 0 && !PlayerManager.noRevive)
            StartCoroutine("Revive");
        //Else, show the finish menu
        else
            ShowFinishMenu();
    }
    //Called, when a mission is completed
    //Updates the sprite of the audio buttons
    private void UpdateAudioButtons()
    {
//        Sprite s = AudioManager.Instance.audioEnabled == true ? audioSprites[0] : audioSprites[1];
//
//        foreach (Image item in audioButtons)
//            item.sprite = s;
    }
    //Updates the shop display texts
    private void UpdateShopDisplay()
    {
        //Update texts

        ShopOwnedItems[0].text = SaveManager.extraSpeed.ToString();
        ShopOwnedItems[1].text = SaveManager.shield.ToString();
        ShopOwnedItems[2].text = SaveManager.sonicWave.ToString();
        ShopOwnedItems[3].text = SaveManager.revive.ToString();



        //If the yellow submarine is active
        if (SaveManager.currentSkinID == 0)
        {
            //Set button sprite
            shopSubmarineButtons[0].sprite = ShopSkinButtonSprites[0];

            //Set sprite for button 2
            if (SaveManager.skin2Unlocked == 1)
                shopSubmarineButtons[1].sprite = ShopSkinButtonSprites[1];
            else
                shopSubmarineButtons[1].sprite = ShopSkinButtonSprites[2];
        }
        //If the green submarine is active
        else
        {
            //Set button sprites
            shopSubmarineButtons[0].sprite = ShopSkinButtonSprites[1];
            shopSubmarineButtons[1].sprite = ShopSkinButtonSprites[0];
        }
    }
    //Shows the finish menu
    private void ShowFinishMenu()
    {
        //Disable main UI, and show the finish menu
        mainUI.gameObject.SetActive(false);
        overlayAnimator.SetBool("Visible", true);
        finishMenuAnimator.gameObject.SetActive(true);
        finishMenuAnimator.SetBool("Visible", true);


        //Set distance and coin text
        finishDistanceText.text = distanceTraveled + "M";

        levelManager.LevelEnded();
        hangarDistanceText.text = SaveManager.bestDistance + " M";

    }
    //Returns true, if the game is in play mode
    public bool InPlayMode()
    {
        return inPlayMode;
    }
    //Converts a number to a string, with a given digit number. For example, this turns 4 to "0004"
    private string AddDigitDisplay(int number, int digits)
    {
        string s = "";

        for (int i = 0; i < digits - number.ToString().Length; i++)
            s += "0";

        s += number.ToString();

        return s;
    }
    //Shows a mission notificator for 2 seconds, then hides it
    private IEnumerator MissionNotificationCountdown(Animator missionNotification, string boolName, int arrayID)
    {
        missionNotification.SetBool(boolName, true);
        yield return new WaitForSeconds(2);
        missionNotification.SetBool(boolName, false);
        usedMissionNotifications[arrayID] = false;
    }
    //Shows the revive button for 3 seconds
    private IEnumerator Revive()
    {
        powerupButtons[3].SetBool("Visible", true);
        yield return new WaitForSeconds(3);

        powerupButtons[3].SetBool("Visible", false);
        ShowFinishMenu();
    }

	public IEnumerator BassCannon ()
	{
		powerupButtons[0].SetBool("Visible", true);
		yield return new WaitForSeconds(3);
		
		powerupButtons[0].SetBool("Visible", false);
	}
    //Disables a specific menu after a given time
    private IEnumerator DisableMenu(Animator menu, float time)
    {
        yield return new WaitForSeconds(time);
        menu.gameObject.SetActive(false);
    }
}





