using UnityEngine;
using System.Collections;

public class ResolutionManager : MonoBehaviour 
{
    public Transform[] toScale;                     //Elements to scale
    public Transform[] toReposition;                //Elements to reposition
	public SpriteRenderer[] groundBackgrounds; 				//Elements to reposition

    public Renderer ground;                           //The renderer of the sand

    private float scaleFactor;                      //The current scale factor

    void Start()
    {
		scaleFactor = Camera.main.aspect / 1.28f;

        //Rescale elements
        foreach (Transform item in toScale)
            item.localScale = new Vector3(item.localScale.x * scaleFactor, item.localScale.y, item.localScale.z);

        //Reposition Elements
        foreach (Transform item in toReposition)
            item.position = new Vector3(item.position.x * scaleFactor, item.position.y, item.position.z);

		for (int i=0; i<3; i++)
		{
			//float xPos = toScale[i-1].position.x + scaleFactor*10;
			//float xPos = toScale[i-1];
			if (i==0)
			{
				float xPos = groundBackgrounds[0].bounds.size.x;
				LevelGenerator.backChangePos = xPos;
			}
			else
			{
				float xPos = groundBackgrounds[i-1].bounds.size.x;
				toScale[i].position = new Vector3(toScale[i-1].position.x + xPos, transform.position.y, transform.position.z);
			}

		}

		//

        //Rescale sand 
       // ground.material.mainTextureScale = new Vector2(scaleFactor, 1);
    }
}
