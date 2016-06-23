#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if ENABLE_ADCOLONY


namespace AppAdvisory.Ads
{
	public class AAADColony : AdBase, IVideoAds, IRewardedVideo
	{

public string Name()
{
return "AAADColony";
}

		public void Init()
		{
			if(FindObjectOfType<ADCAdManagerCustom>() == null)
				gameObject.AddComponent<ADCAdManagerCustom>();
		}

		public bool IsReadyVideoAds()
		{
			return AdColony.IsVideoAvailable(adIds.ADCOLONY_InterstitialVideoZoneID);
		}

		public void CacheVideoAds(){}

		public void ShowVideoAds()
		{
			if(AdColony.IsVideoAvailable(adIds.ADCOLONY_InterstitialVideoZoneID))
			{
				bool showAds = AdColony.ShowVideoAd(adIds.ADCOLONY_InterstitialVideoZoneID);
				print("Ad Colony show interstitiazl video = " + showAds);
				return;
			}
			print("Ad Colony doesn't have interstitiazl video so don't show!!");
		}

		public bool IsReadyRewardedVideo()
		{
			return  AdColony.IsV4VCAvailable(adIds.ADCOLONY_RewardedVideoZoneID);
		}

		public void CacheRewardedVideo(){}

		Action<bool> successRewardedVideoCallback = null;

		void Unsubscribe()
		{
			AdColony.OnV4VCResult -= OnV4VCResult;
			successRewardedVideoCallback = null;
		}

		public void ShowRewardedVideo(Action<bool> success)
		{
			Unsubscribe();

			if(AdColony.IsV4VCAvailable(adIds.ADCOLONY_RewardedVideoZoneID))
			{
				print("adcolony have a rewarded video");
			
				successRewardedVideoCallback = success;
				AdColony.OnV4VCResult += OnV4VCResult;
				AdColony.OfferV4VC(true, adIds.ADCOLONY_RewardedVideoZoneID);

				return;
			}

			print("adcolony have not rewarded video  so don't show!!");
		}

		void OnV4VCResult(bool successRewarded, string name, int amount)
		{
			print("adcolony have callback rewarded video success = " + successRewarded);

			if(successRewardedVideoCallback != null)
				successRewardedVideoCallback(successRewarded);
		}
	}
}

#endif