using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public LayerMovement layerMovement;
	public HydrationBar hydrationBar;
	public Vector3 startPos;
	public float jumpHeight;
	public AudioClip[] audioClips;
	public GameObject tyeDyeBackground;

	private Animator anim;
	private Rigidbody2D rigidBody;
	private bool grounded;
	private bool jumping;
	private bool spring;
	private bool stopJump;
	private bool changingLevels;
	private bool decrease;
	private bool changingOnce;
	private bool holdGround;
	private AudioSource audioSource;

	void Awake ()
	{
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
		startPos = transform.position;
	}

	public void Reset ()
	{
		StopAllCoroutines();
		anim.SetFloat("Handstand", 0f);

		tyeDyeBackground.SetActive(false);
	}
	      
    void Update ()
    {
		if (GameManager.gameStarted && !GameManager.paused && !GameManager.gameOver)
		{
			if (transform.position.y <= -5.25f)
				GameManager.gameOver = true;

            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.05f, whatIsGround);
            Debug.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,groundCheck.position.y - 0.15f,0));
            //ground check
			if (hit == true && !holdGround)
            {
				if (spring || changingLevels)
				{
					audioSource.PlayOneShot(audioClips[5]);
				}
				else if (jumping)
				{
					audioSource.PlayOneShot(audioClips[5]);
				}

				rigidBody.gravityScale = 2.0f;
                grounded = true;
                jumping = false;
				spring = false;
				changingLevels = false;
                anim.SetBool("Jump", false);
                anim.SetBool("Float", false);
				anim.SetBool("Falling", false);
            }
            else //if (!spring && !changingLevels && !jumping)
            {
                anim.SetBool("Jump", true);
                grounded = false;
            }
            //set 
            if (grounded)
                anim.SetBool("Ground", true);
            else
                anim.SetBool("Ground", false);
        
            if (rigidBody.velocity.y < 0.0f)
            {
                if (spring)
				{
					if (layerMovement.speedMultiplier < 1.5f)
						rigidBody.gravityScale = 0.04f;
					else if (layerMovement.speedMultiplier < 1.75f)
						rigidBody.gravityScale = 0.06f;
					else if (layerMovement.speedMultiplier < 2f)
						rigidBody.gravityScale = 0.08f;
					else if (layerMovement.speedMultiplier <= 2f)
						rigidBody.gravityScale = 0.1f;
					else if (layerMovement.speedMultiplier < 2.5f)
						rigidBody.gravityScale = 0.14f;
					else if (layerMovement.speedMultiplier < 3f)
						rigidBody.gravityScale = 0.2f;
					else if (layerMovement.speedMultiplier < 3.5f)
						rigidBody.gravityScale = 0.34f;
					else if (layerMovement.speedMultiplier < 4f)
						rigidBody.gravityScale = 0.4f;
					else if (layerMovement.speedMultiplier < 4.5f)
						rigidBody.gravityScale = 0.5f;
					else if (layerMovement.speedMultiplier < 5f)
						rigidBody.gravityScale = 0.5f;
					else if (layerMovement.speedMultiplier >= 5f)
						rigidBody.gravityScale = 0.5f;

					anim.SetBool("Float", true);
//					spring = false;
//					changingLevels = false;
				}
				if (changingLevels)
				{
					if (changingLevels && !changingOnce)
					{
						changingOnce = true;
						layerMovement.IncreaseLevel();
					}
	
					rigidBody.gravityScale = 0.3f;
					
					anim.SetBool("Float", true);
//					spring = false;
//					changingLevels = false;
				}

				if(!spring && !changingLevels)
				{
					anim.SetBool("Falling", true);
				}


				stopJump = true;
			}	
			else
                stopJump = false;
            
			if (Input.GetMouseButtonDown(0) && !changingLevels && !spring && grounded)
            {
                Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                if (mousePos.x > 0.1f || mousePos.y < 0.9f)
                {
                    jumping = true;
                    rigidBody.velocity = Vector3.up * jumpHeight;
                    grounded = false;
                    anim.SetBool("Jump", true);
					//audioSource.PlayOneShot(audioClips[0]);
				}
			}

			if (Input.GetMouseButtonUp(0) && !changingLevels && !spring)
			{
				if (!stopJump)
				{
                    decrease = true;
                }
            }

			if (decrease)
			{
				if (rigidBody.velocity.y > 0.25f)
					rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y * 0.9f);
				else
				{
					decrease = false;
                    rigidBody.velocity = Vector2.zero;
                }
			}

            //if (changingLevels && transform.position.y <= 5.4f)
            //{
            //    Camera.main.transform.position = new Vector3(0.0f, transform.position.y + 2.755f, Camera.main.transform.position.z);
            //}
        }
    }

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1f);
		holdGround = false;
	}

	void OnTriggerEnter2D (Collider2D myCollider)
	{
		if (myCollider.tag == "Water")
		{
			//			myCollider.gameObject.SetActive(false);
			myCollider.GetComponent<Animator>().SetTrigger("Collect");
			hydrationBar.AddWater();

			audioSource.PlayOneShot(audioClips[0], Random.Range(0.5f, 1f));
		}
		else if (myCollider.tag == "Obstacle")
		{
			hydrationBar.LoseWater();
			audioSource.PlayOneShot(audioClips[7]);
			audioSource.PlayOneShot(audioClips[8]);
		}
		else if (myCollider.tag == "Arch" && !spring)
		{

			//GAME OVER
			anim.SetBool("Dead", true);
			audioSource.PlayOneShot(audioClips[9]);
			//			anim.SetBool("GameOver", true);

			GameManager.gameOver = true;
		}
		else if (myCollider.tag == "Spring")
		{
			myCollider.GetComponent<Animator>().SetBool("Activate", true);

			holdGround = true;
			StartCoroutine(Wait());

			grounded = false;
			rigidBody.velocity = Vector3.up * 13.0f;
			spring = true;
			anim.SetBool("Jump", true);
			audioSource.PlayOneShot(audioClips[1]);
		}
		else if (myCollider.tag == "LevelSpring")
		{
			myCollider.GetComponent<Animator>().SetBool("Activate", true);

			holdGround = true;
			StartCoroutine(Wait());

			changingOnce = false;
			grounded = false;
			rigidBody.velocity = Vector3.up * 23.0f;
			grounded = false;
			changingLevels = true;
			anim.SetBool("Jump", true);
			audioSource.PlayOneShot(audioClips[2]);

			Camera.main.GetComponent<Animator>().SetTrigger("Level");
		}
		else if (myCollider.tag == "Handstand")
		{
			myCollider.gameObject.SetActive(false);
			audioSource.PlayOneShot(audioClips[10]);

			StartCoroutine(Handstand());
		}
		else if (myCollider.tag == "TyeDye")
		{
			myCollider.gameObject.SetActive(false);
			audioSource.PlayOneShot(audioClips[10]);

			StartCoroutine(TyeDye());
		}
		else if (myCollider.tag == "SlowMo")
		{
			myCollider.gameObject.SetActive(false);
			audioSource.PlayOneShot(audioClips[10]);

			StartCoroutine(SlowMo());
		}
	}

	IEnumerator TyeDye ()
	{
		tyeDyeBackground.SetActive(true);
		yield return new WaitForSeconds(10f);
		tyeDyeBackground.SetActive(false);
	}

	IEnumerator SlowMo()
	{
		Time.timeScale = 0.5f;
		yield return new WaitForSeconds(5f);
		Time.timeScale = 1f;
	}

	IEnumerator Handstand()
	{
		anim.SetFloat("Handstand", 1f);
		yield return new WaitForSeconds(10f);
		anim.SetFloat("Handstand", 0f);
	}
}