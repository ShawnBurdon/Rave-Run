#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AppAdvisory.Ads
{
	public class AdsInit : MonoBehaviour 
	{
		public ADIDS adIdds;

		public bool randomize = false;

		public void SetADIDS(ADIDS t)
		{
			this.adIdds = t;
		}

		[SerializeField] public BannerNetwork bannerNetwork = BannerNetwork.NULL ;
		[SerializeField] public List<InterstitialNetwork> interstitialNetworks = new List<InterstitialNetwork>(new InterstitialNetwork[] {InterstitialNetwork.NULL});
		[SerializeField] public List<VideoNetwork> videoNetworks = new List<VideoNetwork>(new VideoNetwork[] {VideoNetwork.NULL});
		[SerializeField] public List<RewardedVideoNetwork> rewardedVideoNetworks = new List<RewardedVideoNetwork>( new RewardedVideoNetwork[] {RewardedVideoNetwork.NULL});

		void Awake()
		{
//			PlayerPrefs.DeleteAll();
//			PlayerPrefs.Save();


			if(FindObjectOfType<AdsManager>() == null)
			{
				
				var o = new GameObject();
				o.SetActive(false);
			
				var adsManager = o.AddComponent<AdsManager>();

				adsManager.enabled = true;

				o.name = "_AdsManager";

				adsManager.adIds = this.adIdds;

				adsManager.bannerNetwork = this.bannerNetwork;
				adsManager.interstitialNetworks = this.interstitialNetworks;
				adsManager.videoNetworks = this.videoNetworks;
				adsManager.rewardedVideoNetworks = this.rewardedVideoNetworks;

				o.SetActive(true);
			}

			Destroy(gameObject);
		}

	}
}