#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0162 

using UnityEngine;
using System.Collections;

namespace AppAdvisory.Ads
{
	public class ADIDS : ScriptableObject
	{

		[SerializeField]
		public bool IsAndroidAmazon = false;

		//CHARTBOOST
		public string ChartboostAppIdIOS = "54e36e3ec909a6251f58dd0d";
		public string ChartboostAppSignatureIOS = "fd5a8192e12eb7342d292c71ee4dc52cb6c5a4bd";

		public string ChartboostAppIdAndroid = "54db41270d6025270134168e";
		public string ChartboostAppSignatureAndroid = "d2bf73b2646e54edd6fbb954f2ab734f69632e6d";

		public string ChartboostAppIdAmazon = "54db3dcc43150f073385bafd";
		public string ChartboostAppSignatureAmazon = "9fd4236bcff00a9949ad39350ebe13c451498ad4";

		public string ChartboostAppId
		{
			get
			{
				#if UNITY_IOS
				return ChartboostAppIdIOS;
				#endif

				#if UNITY_ANDROID
				if(isAmazon)
					return ChartboostAppIdAmazon;
				else
					return ChartboostAppIdAndroid;
				#endif

				#if !UNITY_IOS && !UNITY_ANDROID
				return "";
				#endif
			}
		}

		public string ChartboostAppSignature
		{
			get
			{
				#if UNITY_IOS
				return ChartboostAppSignatureIOS;
				#endif

				#if UNITY_ANDROID
				if(isAmazon)
					return ChartboostAppSignatureAmazon;
				else
					return ChartboostAppSignatureAndroid;
				#endif

				#if !UNITY_IOS && !UNITY_ANDROID
				return "";
				#endif
			}
		}
		//CHARTBOOST

		[SerializeField]
		public bool IsAdmobSettingsOpened = false;
		[SerializeField]
		public bool IsAdmobIOSSettingsOpened = false;
		[SerializeField]
		public bool IsAdmobANDROIDSettingsOpened = false;
		[SerializeField]
		public bool IsAdmobAMAZONSettingsOpened = false;

		[SerializeField]
		public bool IsUnityAdsSettingsOpened = false;

		[SerializeField]
		public bool IsChartboostSettingsOpened = false;
	
		[SerializeField]
		public bool IsADColonySettingsOpened = false;

		[SerializeField]
		public bool rewardedVideoAlwaysReadyInSimulator = false;

		[SerializeField]
		public bool rewardedVideoAlwaysSuccessInSimulator = false;

		[SerializeField]
		public bool ShowIntertitialAtStart
		//= true;
		{
			get
			{
				return PlayerPrefsX.GetBool("_ShowIntertitialAtStart",true);
			}
			set
			{
				PlayerPrefsX.SetBool("_ShowIntertitialAtStart", value);
				PlayerPrefs.Save();
			}
		}


		#if UNITY_ANDROID
		[SerializeField]
		public bool isAmazon = false;
		#endif


		#if ENABLE_ADMOB
		[SerializeField]
		public string AdmobBannerIdIOS = "ca-app-pub-4501064062171971/1747324045";

		[SerializeField]
		public string AdmobInterstitialIdIOS = "ca-app-pub-4501064062171971/3224057246";

		[SerializeField]
		public string AdmobRewardedVideoIdIOS = "ca-app-pub-4501064062171971/3583006040";

		[SerializeField]
		public string AdmobBannerIdANDROID = "ca-app-pub-4501064062171971/7654256841";

		[SerializeField]
		public string AdmobInterstitialIdANDROID = "ca-app-pub-4501064062171971/9130990046";

		[SerializeField]
		public string AdmobRewardedVideoIdANDROID = "ca-app-pub-4501064062171971/5059739248";

		[SerializeField]
		public string AdmobBannerIdAMAZON = "ca-app-pub-4501064062171971/7654256841";

		[SerializeField]
		public string AdmobInterstitialIdAMAZON = "ca-app-pub-4501064062171971/9130990046";

		[SerializeField]
		public string AdmobRewardedVideoIdAMAZON = "ca-app-pub-4501064062171971/5059739248";

		[SerializeField]
		public string admobBannerID
		{
			get
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
					return AdmobBannerIdIOS;
				else
				{
					bool isAmazon = false;

		#if ANDROID_AMAZON
					isAmazon = true;
		#endif
					if(!isAmazon)
						return AdmobBannerIdANDROID;
					else
						return AdmobBannerIdAMAZON;
				}
			}
		}
		[SerializeField]
		public string admobInterstitialID
		{
			get
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
					return AdmobInterstitialIdIOS;
				else
				{
					bool isAmazon = false;

		#if ANDROID_AMAZON
		isAmazon = true;
		#endif

					if(!isAmazon)
						return AdmobInterstitialIdANDROID;
					else
						return AdmobInterstitialIdAMAZON;
				}
			}
		}

		[SerializeField]
		public string admobRewardedVideoID
		{
			get
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
					return AdmobRewardedVideoIdIOS;
				else
				{
					bool isAmazon = false;

		#if ANDROID_AMAZON
		isAmazon = true;
		#endif

					if(!isAmazon)
						return AdmobRewardedVideoIdANDROID;
					else
						return AdmobRewardedVideoIdAMAZON;
				}
			}
		}


		#endif

		#if STAN_ASSET_GOOGLEMOBILEADS
		private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;
		#endif

		#if STAN_ASSET_ANDROIDNATIVE
		private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;
		#endif

		#if UNITY_ADS
		public bool activateRegularInterstitial_UNITY_ADS = false;
		public bool activateRewardedVideo_UNITY_ADS = false;
		public string rewardedVideoZoneUnityAds = "rewardedVideo";
		#endif

		#if ENABLE_ADCOLONY
		public bool activateRegularInterstitial_ADCOLONY = false;
		public bool activateRewardedVideo_ADCOLONY = false;
		//		 Arbitrary version number
		public string version = "1.1";



		[SerializeField] public string AdColonyAppID_iOS = "appdec0ff5be54449c0b0";
	
		public string ADCOLONY_appId
		{
			get
			{
		#if UNITY_IOS
		return AdColonyAppID_iOS;
		#endif

		#if UNITY_ANDROID
				return AdColonyAppID_ANDROID;
		#endif
			}
		}




		[SerializeField] public string AdColonyInterstitialVideoZoneKEY_iOS = "VideoZone1";
		[SerializeField] public string AdColonyInterstitialVideoZoneID_iOS = "vza1dd16e9882640a7b4";

		[SerializeField] public string AdColonyInterstitialVideoZoneKEY_ANDROID = "VideoZone1";
		[SerializeField] public string AdColonyInterstitialVideoZoneID_ANDROID = "vz96292e0ce9c44b728f";


		public string ADCOLONY_InterstitialVideoZoneKEY
		{
			get
			{
		#if UNITY_IOS
		return AdColonyInterstitialVideoZoneKEY_iOS;
		#endif

		#if UNITY_ANDROID
				return AdColonyInterstitialVideoZoneKEY_ANDROID;
		#endif
			}
		}





		public string ADCOLONY_InterstitialVideoZoneID
		{
			get
			{
		#if UNITY_IOS
		return AdColonyInterstitialVideoZoneID_iOS;
		#endif

		#if UNITY_ANDROID
				return AdColonyInterstitialVideoZoneID_ANDROID;
		#endif
			}
		}


		[SerializeField] public string AdColonyAppID_ANDROID = "appb7de26187820418c97";


		[SerializeField] public string AdColonyRewardedVideoZoneKEY_iOS = "V4VCZone1";

		[SerializeField] public string AdColonyRewardedVideoZoneID_iOS = "vzeba48d17b06a43dea6";





		[SerializeField] public string AdColonyRewardedVideoZoneKEY_ANDROID = "V4VCZone1";

		[SerializeField] public string AdColonyRewardedVideoZoneID_ANDROID = "v4vc74c3fdbf7da34df2a4";

		public string ADCOLONY_RewardedVideoZoneID
		{
			get
			{
		#if UNITY_IOS
		return AdColonyRewardedVideoZoneID_iOS;
		#endif

		#if UNITY_ANDROID
				return AdColonyRewardedVideoZoneID_ANDROID;
		#endif
			}
		}

		public string ADCOLONY_RewardedVideoZoneKEY
		{
			get
			{
		#if UNITY_IOS
		return AdColonyRewardedVideoZoneKEY_iOS;
		#endif

		#if UNITY_ANDROID
				return AdColonyRewardedVideoZoneKEY_ANDROID;
		#endif
			}
		}
		#endif




	}
}