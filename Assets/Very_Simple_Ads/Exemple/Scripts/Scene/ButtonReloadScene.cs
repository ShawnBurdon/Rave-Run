using UnityEngine;
using System.Collections;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public class ButtonReloadScene : ButtonBase 
{
	void Start()
	{
		SetText("Reload Scene");
	}

	public override void OnClicked()
	{
		#if UNITY_5_3
		SceneManager.LoadSceneAsync(0,LoadSceneMode.Single);
		#else
		Application.LoadLevelAsync(Application.loadedLevel);
		#endif
	}
}
