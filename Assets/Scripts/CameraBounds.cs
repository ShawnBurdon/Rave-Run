using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour {

	public Camera cam;
	//private float horizonSize = 10.120279522120400965f;

	void OnDrawGizmos()
	{
		float verticalHeightSeen = cam.orthographicSize * 2.0f;
		
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position, new Vector3((verticalHeightSeen * cam.aspect), verticalHeightSeen, 0));
	}

	void Start ()
	{
		//float normalAspect = 9 / 16.0f;
		//float camSize = horizonSize * normalAspect / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight);
		//camSize = Mathf.Round (camSize * 100) / 100;
		//Camera.main.orthographicSize = camSize;

		//gameplay.transform.position = new Vector2(0,cam.ViewportToWorldPoint(new Vector2(0.5f,0)).y + 5f);
		//Debug.Log ((normalAspect)  + " " +((float)Camera.main.pixelWidth / Camera.main.pixelHeight));
		float width = 3.19f / 640.0f * 1136.0f;
		Camera.main.orthographicSize = width / Screen.width * Screen.height;
	}
}