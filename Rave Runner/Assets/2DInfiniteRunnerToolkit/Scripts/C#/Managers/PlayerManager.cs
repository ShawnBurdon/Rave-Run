using UnityEngine;
using System.Collections;

enum PlayerState { Enabled, Disabled };
enum PlayerStatus { Idle, Running, Jumping, Crashed };
enum PlayerVulnerability { Enabled, Disabled, Shielded };
enum PowerupUsage { Enabled, Disabled };

public class PlayerManager : MonoBehaviour
{
	
	//How much vertical force to apply when jumping
	public float jumpForce = 4.0f;
	//The object that checks for ground
	public Transform groundCheck;
	//Colliders on these layers are considered ground
	public LayerMask whatIsGround;
	
	public ChangingLevels changingLevels;  //the script changing levels on the camera
	public LevelGenerator levelGenerator;
	public HydrationBar hydrationBar;
	//public GameObject cloudLayer;
	
	//How far around the object to check
	//private float groundRadius = 0.2f;
	private Animator anim;		//animator
	private bool jump;		//are you jumping
	
	public LevelManager levelManager;                                   //A link to the Level Manager
	
	
	//public Transform extraSpeedFront;                                   //The extra speed front sprite
	//public Transform extraSpeedTail;                                    //The extra speed trail sprite
	//public Transform shield;                                            //The shield sprite
	
	//public ParticleSystem smoke;										//The smoke particle
	//public ParticleSystem bubbles;										//The submarine bubble particle system
	//public ParticleSystem reviveParticle;								//The revive particle


	//public PolygonCollider2D normalCollider;                            //The normal collider of the submarine
	//public CircleCollider2D shieldCollider;                             //The collider of the shield
	
	private PlayerStatus playerStatus;
	private PlayerState playerState;
	private PlayerVulnerability playerVulnerability;
	private PowerupUsage powerupUsage;
	
	private int currentSkinID;
	
	private Vector2 startingPos;                                        //Holds the starting position of the submarine
	
	public bool grounded;												//Is the character grounded
	private float clampX;		//clamp for x position
	private float tapDown;
	private float tapUp;
	private bool jumpPad;

	public static bool gamePaused;
	public static float jumpVariable;
	public static bool stopJump;
	public static bool decrease;
	private bool bassCannon; //are you using bassCannon?
	private Vector3 startBassPos;
	public static bool noRevive;
	public GameObject rainbowBraceletGO;
	private bool nearMiss;
	public int nearMissCount;

	public Collider2D bottomCollider;

	//public bool rainbowBracelet;
	//Used for initialization
	void Start()
	{
		//PlayerPrefs.DeleteAll();

		//rainbowBracelet = true;
		clampX = transform.position.x;
		
		anim = GetComponent<Animator> ();
		
		startingPos = this.transform.position;
		
		playerStatus = PlayerStatus.Idle;
		playerState = PlayerState.Disabled;
		playerVulnerability = PlayerVulnerability.Disabled;
		powerupUsage = PowerupUsage.Disabled;
		
		anim.SetBool("GameStarted", false);
		StartCoroutine(Blink ());
	}
	//Called at every frame


	void FixedUpdate ()
	{
		RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.15f, whatIsGround);
		Debug.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,groundCheck.position.y - 0.15f,0));

		if (hit == true)
			grounded = true;
		else
			grounded = false;

		anim.SetBool ("Ground", grounded);


		if (decrease)
		{
			if (GetComponent<Rigidbody2D>().velocity.y > 0.25f)
				GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, GetComponent<Rigidbody2D>().velocity.y * 0.9f);
			else
			{
				decrease = false;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}
		
		if (GetComponent<Rigidbody2D>().velocity.y <= 0.0f)
		{
			stopJump = true;
		}	
		else
			stopJump = false;
		
		//#endif
		//Clamping position
		transform.position = new Vector3(Mathf.Clamp(transform.position.x,clampX,clampX),Mathf.Clamp(transform.position.y,-15.0f,17.2f),transform.position.z);
		
		if (GetComponent<Rigidbody2D>().velocity.y < 0.0f && transform.position.y > 8.0f)
		{
			changingLevels.SwitchingLevels();
		}
	}

	void Update()
	{    

//		if (rainbowBracelet)
//			rainbowBraceletGO.SetActive(true);
//		else
//			rainbowBraceletGO.SetActive(false);

		//Debug.Log (GetComponent<Rigidbody2D>().velocity);
//#if UNITY_EDITOR || UNITY_STANDALONE
//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//			tapDown = Time.time;
//			jumpVariable = 2.0f;
//			UpdateInput(true, jumpVariable);
//		}	
//		if (Input.GetKeyUp(KeyCode.Space))
//		{			
//			if (!stopJump)
//			{
//				tapUp = Time.time - tapDown;
//				tapUp += 1.0f;
//
//				if (tapUp >= 2.0f)
//					tapUp = 2.0f;
//
//				jumpVariable = tapUp;
//				decrease = true;
//			}
//		}
//#endif


//
//		if (SaveManager.rainbowBraceletUnlocked == 1)
//			rainbowBraceletGO.SetActive(true);
//		else
//			rainbowBraceletGO.SetActive(false);

		//decrease velocity to just above zero once you let up on tap


		//		Debug.Log (playerState);
		//    	Debug.Log (grounded);
		//    	Debug.Log (anim.GetBool("Ground"));
		//		Debug.Log (GetComponent<Rigidbody2D>().velocity.y);
		

		//		if (grounded && transform.position.y > 5.0f)
		//		{
		//			changingLevels.Clouds();
		//		}
		
		
		//		if (transform.position.y > 10.8f)
		//		{
		//			cloudLayer.GetComponent<BoxCollider2D>().enabled = true;
		//		}
		
		// Check if the character is touching the ground



		// Check the vertical speed of the character 
		if (playerStatus != PlayerStatus.Crashed)
			anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
		
		// Sets animations
//		if (playerStatus == PlayerStatus.Running && playerStatus != PlayerStatus.Crashed)
//			anim.SetBool ("Ground", grounded);
		//    	else if (playerStatus == PlayerStatus.Crashed)
		//			anim.SetBool("Dead", true);
		

		
		if (playerState == PlayerState.Enabled)
		{
			if (hydrationBar.waterBar.fillAmount == 0.0f)
				Crash ();

			if (grounded && !jump && playerStatus != PlayerStatus.Crashed)
				playerStatus = PlayerStatus.Running;
			else if (grounded && jump) //
			{
				playerStatus = PlayerStatus.Jumping;
				anim.SetBool ("Ground", grounded);
				changingLevels.Grounded();
				jump = false;
				grounded = false;
				Jump(jumpForce, 2.0f);
			}
			if (grounded)
				changingLevels.Grounded();

			if (playerStatus == PlayerStatus.Crashed)
			{
				Crash();
			}
			
			if (transform.position.y < -12 || Input.GetKeyDown(KeyCode.A))
			{
				Crash ();
				noRevive = true;
			}
		}



	}
	
	public void Jump (float jForce, float jumpVar)
	{	
		if (!bassCannon)
			GetComponent<Rigidbody2D>().velocity =  new Vector3(0,jForce,0) * jumpVar;
	}
	
	//Called when the player enters a triggerer zone
	void OnTriggerEnter2D(Collider2D myCollider)
	{
		//Debug.Log (myCollider.tag);
		//If the submarine is collided with a coin
		if (myCollider.tag == "Coin")
		{
			//Debug.Log ("PM coin");
			//Notify the level manager, and disable the coin's renderer and collider
			levelManager.CoinCollected(myCollider.transform.position, 1);
			myCollider.GetComponent<Renderer>().enabled = false;
			myCollider.GetComponent<Collider2D>().enabled = false;
			
			AudioManager.Instance.PlayCoinCollected();
		}

//		if (myCollider.tag == "Candy")
//		{
//			Candy(myCollider.transform.position);
//
//			myCollider.GetComponent<Renderer>().enabled = false;
//			myCollider.GetComponent<Collider2D>().enabled = false;
//			myCollider.transform.FindChild("Trail").gameObject.SetActive(false);
//		}
		//If the submarine is collided with an obstacle
		else if (myCollider.tag == "Obstacle")
		{
//			if (nearMiss)
//			{
//				nearMiss = false;
//				nearMissCount--;
//			}

			//Notify the level manager, and disable the obstacle's renderer and collider
			levelManager.Collision(myCollider.gameObject.name, myCollider.transform.position);
			myCollider.gameObject.GetComponent<Renderer>().enabled = false;
			myCollider.gameObject.GetComponent<Collider2D>().enabled = false;
			
			AudioManager.Instance.PlayExplosion();
			
			//If the obstacle is a torpedo, disable it's child as well
			if (myCollider.name == "Torpedo")
				myCollider.transform.FindChild("TorpedoFire").gameObject.SetActive(false);

			//Change multiplier speed.

//			if (playerVulnerability == PlayerVulnerability.Enabled)
//				hydrationBar.LoseHydration(0.1f);
//
//			//If the player is vulnerable
//			if (playerVulnerability == PlayerVulnerability.Enabled && hydrationBar.waterBar.fillAmount == 0.0f)
//			{
//				//Sink the submarine
//				powerupUsage = PowerupUsage.Disabled;
//				playerVulnerability = PlayerVulnerability.Disabled;
//				playerStatus = PlayerStatus.Crashed;
//			}
			//If the player is shielded, collapse it
			else if (playerVulnerability == PlayerVulnerability.Shielded)
			{
				//CollapseShield();
			}

			else if (playerVulnerability == PlayerVulnerability.Enabled && hydrationBar.waterBar.fillAmount > 0.0f && !LevelGenerator.dead)
			{

				levelGenerator.HitObstacle();
			}
		}

		//If the submarine is collided with a powerup
//		else if (myCollider.tag == "Water")
//		{
//			//Notify the level manager, and disable the powerup's components
//			myCollider.GetComponent<Renderer>().enabled = false;
//			myCollider.GetComponent<Collider2D>().enabled = false;
//			myCollider.transform.FindChild("Trail").gameObject.SetActive(false);
//			
//			AudioManager.Instance.PlayPowerupCollected();
//			hydrationBar.AddWater();
//		}
	}

	public void NearMiss (bool near)
	{
		nearMiss = near;

		if (nearMiss && playerState == PlayerState.Enabled)
		{
			nearMissCount++;
			nearMiss = false;
		}
	}


	//Enables the character
	public void EnableCharacter()
	{
		anim.SetBool("GameStarted", true);
		playerStatus = PlayerStatus.Idle;
		playerState = PlayerState.Enabled;
		playerVulnerability = PlayerVulnerability.Enabled;
		powerupUsage = PowerupUsage.Enabled;
//		hydrationBar.Hydrating();
		grounded = true;
	}
	//Sets the pause state of the submarine
	public void SetPauseState(bool pauseState)
	{
		if (pauseState)
		{
			Time.timeScale = 0.0f;
			//pausedPos = transform.position;
			//hydrationBar.Paused();
			//playerState = PlayerState.Disabled;
			//playerStatus = PlayerStatus.Idle;
			//gamePaused = true;
		}
		else
		{
			Time.timeScale = 1.0f;
			//hydrationBar.NotPaused();
			//playerState = PlayerState.Enabled;
			//gamePaused = false;
		}
	}
	//Changes the active skin ID
	public void ChangeSkin(int id)
	{
		//		currentSkinID = id;
		//
		//		if (playerStatus != PlayerStatus.Crashed)
		//			subRenderer.sprite = subTextures[currentSkinID * 2 + 1];
	}
	//Resets the submarine
	public void Reset()
	{
		playerState = PlayerState.Disabled;
		gamePaused = false;
		playerVulnerability = PlayerVulnerability.Disabled;
		powerupUsage = PowerupUsage.Disabled;
		anim.SetBool("Dead", false);
		anim.SetTrigger("Reset");
		grounded = true;
		playerStatus = PlayerStatus.Running;
//		hydrationBar.Hydrating();
		anim.SetBool ("Ground", grounded);
		nearMissCount = 0;
				
		this.transform.position = startingPos;
		changingLevels.Reset();
	}
	//Revives the submarine
	public void Revive()
	{
		StartCoroutine("ReviveProcess");
	}
	//Updates player input
	public void UpdateInput(bool inputActive, float jumpMult)
	{
		if (inputActive)
		{
			if (GUIManager.inPlayMode && grounded  && playerStatus != PlayerStatus.Crashed)
			{
				jump = true;
				jumpVariable = jumpMult;
			}
		}
	}
	
	//Activates the extra speed submarine effects
    public void ActivateExtraSpeed()
    {
//	        extraSpeedFront.gameObject.SetActive(true);
//	        extraSpeedTail.gameObject.SetActive(true);
//	        RaiseShield();
		startBassPos = transform.position;
		bassCannon = true;
        playerVulnerability = PlayerVulnerability.Disabled;
        powerupUsage = PowerupUsage.Disabled;
    }
//Deactivates the extra speed submarine effects
    public void DisableExtraSpeed()
    {
//	        extraSpeedFront.gameObject.SetActive(false);
//	        extraSpeedTail.gameObject.SetActive(false);
//	        CollapseShield();
		bassCannon = false;
		playerVulnerability = PlayerVulnerability.Enabled;
        powerupUsage = PowerupUsage.Enabled;
    }
	//Raises the shield
	//    public void RaiseShield()
	//    {
	//        playerVulnerability = PlayerVulnerability.Shielded;
	//
	//        normalCollider.enabled = false;
	//        shieldCollider.enabled = true;
	//
	//        StartCoroutine(FunctionLibrary.ScaleTo(shield, new Vector2(1, 1), 0.25f));
	//    }
	//    //Collapses the shield
	//    public void CollapseShield()
	//    {
	//        playerVulnerability = PlayerVulnerability.Disabled;
	//
	//        normalCollider.enabled = true;
	//        shieldCollider.enabled = false;
	//
	//        StartCoroutine(FunctionLibrary.ScaleTo(shield, new Vector2(0, 0), 0.25f));
	//        StartCoroutine(EnableVulnerability(0.3f));
	//    }
	//Returns true, if a powerup can be activated
	public bool CanUsePowerup()
	{
		return playerState == PlayerState.Enabled && powerupUsage == PowerupUsage.Enabled;
	}
	
	//Sinks the submarine until it crashes to the sand
	private void Crash()
	{
		anim.SetBool("Dead", true);
		playerState = PlayerState.Disabled;
		playerStatus = PlayerStatus.Crashed;
		LevelGenerator.dead = true;
		levelManager.StopLevel();
	}
	
	//Enables player vulnerability after time
	private IEnumerator EnableVulnerability(float time)
	{
		//Declare variables, get the starting position, and move the object
		float i = 0.0f;
		float rate = 1.0f / time; 
		
		while (i < 1.0)
		{
			//If the game is not paused, increase t
			if (playerState == PlayerState.Enabled)
				i += Time.deltaTime * rate;
			
			//Wait for the end of frame
			yield return 0;
		}
		
		playerVulnerability = PlayerVulnerability.Enabled;
	}
	
	IEnumerator Blink ()
	{
		int i = Random.Range(0,10);
		
		if (i > 6)
			anim.SetFloat("Blink", 1.0f);
		else if (i < 3)
			anim.SetFloat("Blink", 0.0f);
		else
			anim.SetFloat("Blink", 0.5f);
		
		yield return new WaitForSeconds (Random.Range(1.5f, 3.0f));
		
		StartCoroutine(Blink());
	}

	//Revives the player, and moves the submarine upward
	private IEnumerator ReviveProcess()
    {
        //Change the texture to intact, and play the revive particle
		anim.SetBool ("Dead", false);
        //Reset states
        playerStatus = PlayerStatus.Idle;
        playerState = PlayerState.Enabled;
       
//		hydrationBar.Hydrating();

        yield return new WaitForSeconds(0.5f);
        powerupUsage = PowerupUsage.Enabled;

		yield return new WaitForSeconds(1.5f);
		playerVulnerability = PlayerVulnerability.Enabled;
    }
}