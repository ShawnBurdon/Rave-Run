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

#if ENABLE_ADMOB

using GoogleMobileAds.Api;

namespace AppAdvisory.Ads
{
	public class AAAdmob : AdBase, IInterstitial, IBanner, IRewardedVideo
	{
		public string bannerId
		{
			get
			{
				return adIds.admobBannerID;
			}
		}

		public string interstitialID
		{
			get
			{
				return adIds.admobInterstitialID;
			}
		}

		public string rewardedVideoID
		{
			get
			{
				return adIds.admobRewardedVideoID;
			}
		}

		public string Name()
		{
			return "AAAdmob";
		}

		public void Init()
		{
			if(bannerId != null && string.IsNullOrEmpty(bannerId) == false)
				Debug.LogWarning("AAAdmob - Init bannerId = " + bannerId);

			if(interstitialID != null && string.IsNullOrEmpty(interstitialID) == false)
				Debug.LogWarning("AAAdmob - Init interstitialID = " + interstitialID);

			if(rewardedVideoID != null && string.IsNullOrEmpty(rewardedVideoID) == false)
				Debug.LogWarning("AAAdmob - Init rewardedVideoID = " + rewardedVideoID);

			RequestBanner();

			RequestInterstitial();
		}

		BannerView bannerView;
		InterstitialAd interstitial;

		private void RequestBanner()
		{
			if(!string.IsNullOrEmpty(bannerId))
			{
				Debug.LogWarning("AAAdmob - RequestBanner with bannerid = " + bannerId);	
				
				bannerView = new BannerView(bannerId, AdSize.SmartBanner, AdPosition.Bottom);
				bannerView.LoadAd(createAdRequest());
				bannerView.Hide();
			}
			else
			{
				Debug.LogWarning("AAAdmob - RequestBanner ERROR ID IS NULL!!");	
			}
		}
	
		private void RequestInterstitial(Action<bool> onAdLoadSuccess)
		{

			if(!string.IsNullOrEmpty(interstitialID))
			{
				Debug.LogWarning("AAAdmob - RequestInterstitial with interstitialID = " + interstitialID);

				interstitial = new InterstitialAd(interstitialID);
				interstitial.OnAdLoaded += delegate(object sender, EventArgs e) {
					print("interstitial.OnAdLoaded");
					if(onAdLoadSuccess != null)
						onAdLoadSuccess(true);
				};
				interstitial.OnAdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs e) {
					print("interstitial.OnAdFailedToLoad");
					if(onAdLoadSuccess != null)
						onAdLoadSuccess(false);
				};
//				interstitial.OnAdOpening += delegate(object sender, EventArgs e) {
//					print("interstitial.OnAdOpening");
//				};
//				interstitial.OnAdClosed += delegate(object sender, EventArgs e) {
//					print("interstitial.OnAdClosed");
//				};
//				interstitial.OnAdLeavingApplication += delegate(object sender, EventArgs e) {
//					print("interstitial.OnAdLeavingApplication");
//				};
				interstitial.LoadAd(createAdRequest());
			}
			else
			{
				Debug.LogWarning("AAAdmob - RequestInterstitial ERROR ID IS NULL!!");	

			}
		}

		private void RequestInterstitial()
		{
			RequestInterstitial(null);
		}

		private AdRequest createAdRequest()
		{

			return new AdRequest.Builder()
//				.AddTestDevice(AdRequest.TestDeviceSimulator)
//				.AddTestDevice("8fa42327347fc830609a54a833e611ed1cc716a7")
				.AddKeyword("game")
				.TagForChildDirectedTreatment(false)
				.Build();
		}

		public void ShowBanner()
		{
			Debug.LogWarning("AAAdmob - ShowBanner");
			if(bannerView == null)
			{
				Debug.LogWarning("AAAdmob - ShowBanner bannerView == null ===> requestBanner");
				RequestBanner();

				bannerView.OnAdLoaded += delegate(object sender, EventArgs e) {
					Debug.LogWarning("AAAdmob - ShowBanner bannerView == null ===> requestBanner delegate -----> banner is loaded ----> show");
					bannerView.Show();
				};
			}
			else
			{
				Debug.LogWarning("AAAdmob - ShowBanner bannerView != null ----> show");
				bannerView.Show();
			}
		}

		public void HideBanner()
		{
			Debug.LogWarning("AAAdmob - HideBanner");

			if(bannerView != null)
				bannerView.Hide();
		}
		public void DestroyBanner()
		{
			Debug.LogWarning("AAAdmob - DestroyBanner");

			if(bannerView != null)
			{
				bannerView.Destroy();
			}
		}
		public bool IsReadyInterstitial()
		{
			bool isOK = false;

			if(interstitial != null)
			{
				Debug.LogWarning("Interstitial == null");

				isOK = interstitial.IsLoaded();
			}

			Debug.LogWarning("AAAdmob - IsReadyInterstitial : " + isOK);


//			if(!isOK)
//			{
//				CacheInterstitial();
//			}

			return isOK;
		}

		public bool IsReadyInterstitialStartup()
		{
			return IsReadyInterstitial();
		}

		public void CacheInterstitial()
		{
			Debug.LogWarning("AAAdmob - CacheInterstitial");

			if(interstitial == null)
			{
				RequestInterstitial();
				return;
			}
			else
			{
				if(interstitial != null && !interstitial.IsLoaded())
				{
					interstitial.LoadAd( new AdRequest.Builder().Build());
				}
				else
				{
					RequestInterstitial();
				}
			}
		}

		public void CacheInterstitialStartup()
		{
			CacheInterstitial();
		}

		public void ShowInterstitial(Action<bool> success)
		{
			

			if(interstitial != null && !interstitial.IsLoaded())
			{
				Debug.LogWarning("AAAdmob - ShowInterstitial - is loaded ===> showing");

				interstitial.Show();

				if(success != null)
					success(true);
			}
			else
			{
				Debug.LogWarning("AAAdmob - ShowInterstitial - is NOT loaded ===> request...");
				RequestInterstitial((bool isSuccess) => {
					Debug.LogWarning("AAAdmob - ShowInterstitial - is NOT loaded ===> ... and showing");
					if(isSuccess)
					{
						interstitial.Show();
						if(success != null)
							success(true);
					}
					else
					{
						if(success != null)
							success(false);
					}
				});
			}
		}

		public void ShowInterstitialStartup(Action<bool> success)
		{
			ShowInterstitial(success);
		}


		public void CacheRewardedVideo()
		{
			print("AAAdmob - CacheRewardedVideo");

			RewardBasedVideoAd.Instance.OnAdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs e) {
				Invoke("CacheRewardedVideo",3);
			};

			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			RewardBasedVideoAd.Instance.LoadAd(request,rewardedVideoID);

		}

		public bool IsReadyRewardedVideo()
		{

			bool isOK = RewardBasedVideoAd.Instance.IsLoaded();


			if(!isOK)
			{
				CacheRewardedVideo();
			}

			print("AAAdmob - IsReadyRewardedVideo : " + isOK.ToString());

			return isOK;
		}

		public void ShowRewardedVideo(Action<bool> success)
		{

			if(!IsReadyRewardedVideo())
			{
				if(success != null)
					success(false);
				return;
			}

			var f = FindObjectsOfType<AudioSource>();

			var l = new List<AudioSource>();

			RewardBasedVideoAd.Instance.OnAdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs e) {
				if(success != null)
					success(false);

				if(f != null)
				{
					foreach(var a in f)
					{
						if(a.isPlaying)
						{
							l.Add(a);
							a.mute = false;
						}
					}
				}
			};

			RewardBasedVideoAd.Instance.OnAdOpening += delegate(object sender, EventArgs e) {
				print("opening ad");

				if(f != null)
				{
					foreach(var a in f)
					{
						if(a.isPlaying)
						{
							l.Add(a);
							a.mute = true;
						}
					}
				}
			};

			RewardBasedVideoAd.Instance.OnAdClosed += delegate(object sender, EventArgs e) {
				print("closed ad");

				if(success != null)
					success(false);
				
				if(f != null)
				{
					foreach(var a in f)
					{
						if(a.isPlaying)
						{
							l.Add(a);
							a.mute = false;
						}
					}
				}
			};

			RewardBasedVideoAd.Instance.OnAdStarted += delegate(object sender, EventArgs e) {
				print("ad started");
		
				foreach(var a in f)
				{
					if(a.isPlaying)
					{
						l.Add(a);
						a.mute = true;
					}
				}
			};

			RewardBasedVideoAd.Instance.OnAdRewarded += delegate(object sender, Reward e) 
			{
				print("ad rewarded");

				if(success != null)
					success(true);

				if(f != null)
				{
					foreach(var a in f)
					{
						if(a.isPlaying)
						{
							l.Add(a);
							a.mute = false;
						}
					}
				}
			};

			RewardBasedVideoAd.Instance.Show();
		}
	}
}

#endif