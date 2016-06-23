using UnityEngine;
using System.Collections;
using AppAdvisory.Ads;

public class ButtonShowRewardedVideo : ButtonBase 
{
	void Start()
	{
		SetText("Show Rewarded Video");
	}

	public override void OnClicked()
	{
		#if APPADVISORY_ADS
		AdsManager.instance.ShowRewardedVideo(delegate(bool obj) {
			print("rewarded video success ? ===> " + obj);
		});
		#endif
	}
}
