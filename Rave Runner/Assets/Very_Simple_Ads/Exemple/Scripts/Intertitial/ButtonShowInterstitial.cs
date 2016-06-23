using UnityEngine;
using System.Collections;
using AppAdvisory.Ads;

public class ButtonShowInterstitial : ButtonBase 
{
	void Start()
	{
		SetText("Show Banner");
	}

	public override void OnClicked()
	{
		#if APPADVISORY_ADS
		AdsManager.instance.ShowInterstitial();
		#endif
	}
}
