#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used


using UnityEngine;
using System.Collections;
using System;

#if CHARTBOOST
using ChartboostSDK;

namespace AppAdvisory.Ads
{
	public class AAChartboost : AdBase, IInterstitial, IRewardedVideo
	{
		public string Name()
		{
			return "AAChartboost";
		}

		public void Init()
		{
			print("AAChartboost - Init");

			CBSettings settings = ScriptableObject.CreateInstance<CBSettings>();


			string amazonAppId = adIds.ChartboostAppIdAmazon;
			string amazonAppSecret = adIds.ChartboostAppSignatureAmazon;

			string androidAppId = adIds.ChartboostAppIdAndroid;
			string androidAppSecret = adIds.ChartboostAppSignatureAndroid;



			settings.iOSAppId = adIds.ChartboostAppIdIOS;
			settings.iOSAppSecret = adIds.ChartboostAppSignatureIOS;

			settings.SetIOSAppId(adIds.ChartboostAppIdIOS);
			settings.SetIOSAppSecret(adIds.ChartboostAppSignatureIOS);







			#if ANDROID_AMAZON
			SetAndroidIds(settings, amazonAppId, amazonAppSecret);
			#else
			SetAndroidIds(settings, androidAppId, androidAppSecret);
			#endif








			var c = FindObjectOfType<Chartboost>();

			if(c == null)
			{
				gameObject.AddComponent<Chartboost>();
			}

			Chartboost.setAutoCacheAds(true);
			Chartboost.cacheInterstitial (CBLocation.Default);
			Chartboost.cacheInterstitial (CBLocation.Startup);
			Chartboost.cacheRewardedVideo(CBLocation.Default);
		}

		void SetAndroidIds(CBSettings settings, string appId, string appSecret)
		{
			settings.amazonAppId = appId;
			settings.amazonAppSecret = appSecret;

			settings.SetAmazonAppId(appId);
			settings.SetAmazonAppSecret(appSecret);



			settings.androidAppId = appId;
			settings.androidAppSecret = appSecret;

			settings.SetAndroidAppId(appId);
			settings.SetAndroidAppSecret(appSecret);
		}

		private bool _IsReadyInterstitial(CBLocation location)
		{
			bool isOK = Chartboost.hasInterstitial(location);

			if(!isOK)
			{
				Chartboost.cacheInterstitial(location);
			}

			return isOK;
		}
			
		public bool IsReadyInterstitial()
		{
			return _IsReadyInterstitial(CBLocation.Default);
		}

		public bool IsReadyInterstitialStartup()
		{
			return _IsReadyInterstitial(CBLocation.Startup);
		}

		public void CacheInterstitial()
		{
			Chartboost.cacheInterstitial(CBLocation.Default);
			Chartboost.didCacheInterstitial += delegate(CBLocation obj) {
				print("AAChartboost - CacheInterstitial - didCacheInterstitial");
			};
		}

		public void CacheInterstitialStartup()
		{
			Chartboost.cacheInterstitial(CBLocation.Startup);
			Chartboost.didCacheInterstitial += delegate(CBLocation obj) {
				print("AAChartboost - CacheInterstitialStartup - didCacheInterstitial");
			};
		}

		public void ShowInterstitial(Action<bool> success)
		{
			Chartboost.showInterstitial (CBLocation.Default);
			Chartboost.didDisplayInterstitial += delegate(CBLocation obj) {
				print("AAChartboost - ShowInterstitial - didDisplayInterstitial");
				if(success != null)
					success(true);
			};
			Chartboost.didFailToLoadInterstitial += delegate(CBLocation arg1, CBImpressionError arg2) {
				print("AAChartboost - ShowInterstitial - didFailToLoadInterstitial");
				if(success != null)
					success(false);
			};
		}

		public void ShowInterstitialStartup(Action<bool> success)
		{
			Chartboost.showInterstitial (CBLocation.Startup);
			Chartboost.didDisplayInterstitial += delegate(CBLocation obj) {
				print("AAChartboost - ShowInterstitialStartup - didDisplayInterstitial");
				if(success != null)
					success(true);
			};
			Chartboost.didFailToLoadInterstitial += delegate(CBLocation arg1, CBImpressionError arg2) {
				print("AAChartboost - ShowInterstitialStartup - didFailToLoadInterstitial");
				if(success != null)
					success(false);
			};
		}

		public bool IsReadyRewardedVideo()
		{
			bool isOK =  Chartboost.hasRewardedVideo(CBLocation.Default);

//			if(!isOK)
//			{
//				Chartboost.cacheRewardedVideo(CBLocation.Default);
//			}
//
			print("AAChartboost - IsReadyRewardedVideo : " + isOK.ToString());

			return isOK;
		}

		public void CacheRewardedVideo()
		{
			print("AAChartboost - CacheRewardedVideo");
			Chartboost.cacheRewardedVideo(CBLocation.Default);
		}
		public void ShowRewardedVideo(Action<bool> success)
		{
			Chartboost.showRewardedVideo(CBLocation.Default);
			Chartboost.didFailToLoadRewardedVideo += delegate(CBLocation arg1, CBImpressionError arg2) 
			{
				Debug.Log ("user fail chartboost rewarded video - didFailToLoadRewardedVideo");

				print("AAChartboost - didFailToLoadRewardedVideo");

				if (success != null)
					success (false);
			};

			Chartboost.didCompleteRewardedVideo += delegate(CBLocation arg1, int arg2) 
			{
				Debug.Log ("user success chartboost rewarded video - didCompleteRewardedVideo");

				if (success != null)
					success (true);
			};

			Chartboost.didDismissRewardedVideo += delegate(CBLocation obj) 
			{
				Debug.Log ("user success chartboost rewarded video - didDismissRewardedVideo");

				if (success != null)
					success (false);
			};
		}
	}
}

#endif