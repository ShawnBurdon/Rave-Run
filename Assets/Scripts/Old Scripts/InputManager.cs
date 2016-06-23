using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Player player;         //A link to the Player Manager
	private float tapDown;
	private float tapUp;

	public Text click;

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log( "Down");

		tapDown = Time.time;
		PlayerManager.jumpVariable = 2.0f;
//		Player.UpdateInput(true, PlayerManager.jumpVariable);
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		click.text = "Up";

		if (!PlayerManager.stopJump)
		{
			tapUp = Time.time - tapDown;
			tapUp += 1.0f;
			
			if (tapUp >= 2.0f)
				tapUp = 2.0f;
			
			PlayerManager.jumpVariable = tapUp;
			PlayerManager.decrease = true;
		}
	}
}
