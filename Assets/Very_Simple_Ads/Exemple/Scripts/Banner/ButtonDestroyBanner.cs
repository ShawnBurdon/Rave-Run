using UnityEngine;
using System.Collections;
using AppAdvisory.Ads;

public class ButtonDestroyBanner : ButtonBase 
{
	void Start()
	{
		SetText("Destroy Banner");
	}

	public override void OnClicked()
	{
		#if APPADVISORY_ADS
		AdsManager.instance.DestroyBanner();
		#endif
	}
}
