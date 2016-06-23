using UnityEngine;
using System.Collections;
using AppAdvisory.Ads;

public class ButtonShowBanner : ButtonBase 
{
	void Start()
	{
		SetText("Show Banner");
	}

	public override void OnClicked()
	{
		#if APPADVISORY_ADS
		AdsManager.instance.ShowBanner();
		#endif
	}
}
