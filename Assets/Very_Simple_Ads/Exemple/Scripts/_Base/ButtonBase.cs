using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonBase : MonoBehaviour 
{
	Button b;

	Text t;

	void OnEnable()
	{
		b = GetComponent<Button>();
		b.onClick.AddListener(OnClicked);

		t = GetComponentInChildren<Text>();
	}

	void OnDisable()
	{
		b.onClick.RemoveAllListeners();
	}


	public virtual void OnClicked(){}

	public void SetText(string s)
	{
		t.text = s;
	}
}
