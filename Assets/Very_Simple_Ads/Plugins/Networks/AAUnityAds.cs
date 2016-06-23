#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used


using UnityEngine;
using System.Collections;
using System;

#if UNITY_ADS

using UnityEngine.Advertisements;

namespace AppAdvisory.Ads
{
	public class AAUnityAds : AdBase, IVideoAds, IRewardedVideo
	{
public string Name()
{
return "AAUnityAds";
}

		public void Init(){}
		public void CacheRewardedVideo(){}
		public void CacheVideoAds(){}

		public void ShowVideoAds()
		{
			if (IsReadyVideoAds())
			{
				Advertisement.Show();
			}
		}

		public void ShowRewardedVideo(Action<bool> success)
		{
			if (IsReadyRewardedVideo())
			{
				var options = new ShowOptions { resultCallback = delegate(ShowResult result) {
						switch (result)
						{
						case ShowResult.Finished:
							Debug.Log("UNITY_ADS The ad was successfully shown.");
							if(success!=null)
								success(true);
							break;
						case ShowResult.Skipped:
							Debug.Log("UNITY_ADS The ad was skipped before reaching the end.");
							if(success!=null)
								success(false);
							break;
						case ShowResult.Failed:
							Debug.LogWarning("UNITY_ADS The ad failed to be shown.");
							if(success!=null)
								success(false);
							break;		
						}
					}};
				Advertisement.Show(adIds.rewardedVideoZoneUnityAds, options);
			}
		}
			

		public bool IsReadyVideoAds()
		{
			return Advertisement.IsReady();
		}

		public bool IsReadyRewardedVideo()
		{
			return Advertisement.IsReady(adIds.rewardedVideoZoneUnityAds);
		}
	}
}

#endif