#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used

/*! \mainpage GRAVITY BALL - ASSET STORE - DOCUMENTATION
 * 
 * \section intro_sec Thanks for your purchase
 * 
 * Video tutorial :
 * 
 * https://youtu.be/p8BOs5alLt0
 * 
 * \section intro_sec2 ADS INTEGRATION
 * 
 * Ads Documentation available here:
 * 
 * https://dl.dropboxusercontent.com/u/8289407/GravityBall/Documentation/_Ads_Integration_Documentation.pdf
 * 
 * \section intro_sec5 CLASS DESCRIPTIONS
 * 
 * All you need to know if in the section "Classes":
 * 
 * https://dl.dropboxusercontent.com/u/8289407/GravityBall/Documentation/html/annotated.html
 * 
 * \section intro_sec3 A QUESTION?
 * 
 * If you have any question: 
 * 
 * contact@app-advisory.com
 * 
 * \section intro_sec4 TO HELP US
 * 
 * Please rate my file, I’d appreciate it:
 * 
 * http://u3d.as/mns
 * 
 * \section intro_sec7 FOLLOW US ON FACEBOOK:
 * 
 * Facebook: https://www.facebook.com/appadvisory/
 * 
 * \section intro_sec6 FOLLOW US ON TWITTER:
 * 
 * Twitter: https://twitter.com/AppAdvisory
 * 
 * \section intro_sec8 MORE ASSET FROM US:
 * 
 * https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8911
 */

//#define IAD
//#define ENABLE_ADMOB
//#define GOOGLE_MOBILE_ADS
//#define STAN_ASSET_GOOGLEMOBILEADS
//#define STAN_ASSET_ANDROIDNATIVE
//#define CHARTBOOST
//#define ENABLE_ADCOLONY
//#define ADCOLONY_INTERSTITIAL
//#define ADCOLONY_REWARDED_VIDEO

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif




namespace AppAdvisory.Ads
{
	/// <summary>
	/// This is a static class that references a singleton object in order to provide a simple interface for easely configure Ads in the game
	///
	/// Class in charge to display ads in the game (banners, interstitials and rewarded videos) - please refere to the ADS_INTEGRATION_DOCUMENTATION.PDF
	/// </summary>
	public class AdsManager : AppAdvisory.Ads.Singleton<AdsManager>
	{
		protected AdsManager () {} // guarantee this will be always a singleton only - can't use the constructor!


		[SerializeField] public BannerNetwork bannerNetwork;
		[SerializeField] public List<InterstitialNetwork> interstitialNetworks = new List<InterstitialNetwork>();
		[SerializeField] public List<VideoNetwork> videoNetworks = new List<VideoNetwork>();
		[SerializeField] public List<RewardedVideoNetwork> rewardedVideoNetworks = new List<RewardedVideoNetwork>();

		IBanner banner;
		List<IInterstitial> listInterstitials = new List<IInterstitial>();
		List<IVideoAds> listVideos = new List<IVideoAds>();
		List<IRewardedVideo> listRewardedVideos = new List<IRewardedVideo>();

		/// <summary>
		/// To store the time and know when we have to show an interstitial at game over if, and only if, basedTimeInterstitialAtGameOver = true
		/// </summary>
		float realTimeSinceStartup;

		[SerializeField] public ADIDS adIds;


		#if ENABLE_ADMOB
		void AddAdmob()
		{
			if(gameObject.GetComponent<AAAdmob>() == null)
				gameObject.AddComponent<AAAdmob>();

			GetComponent<AAAdmob>().Init();
		}
		#endif

		#if UNITY_ADS
		void AddUnityAds()
		{
			if(gameObject.GetComponent<AAUnityAds>() == null)
				gameObject.AddComponent<AAUnityAds>();
		}
		#endif

		#if ENABLE_ADCOLONY
		void AddADColony()
		{
			if(gameObject.GetComponent<AAADColony>() == null)
				gameObject.AddComponent<AAADColony>();

			GetComponent<AAADColony>().Init();
		}
		#endif

		#if CHARTBOOST
		void AddChartboost()
		{
			if(gameObject.GetComponent<AAChartboost>() == null)
				gameObject.AddComponent<AAChartboost>();

			GetComponent<AAChartboost>().Init();
		}
		#endif


		void Awake()
		{
			var a = FindObjectsOfType<AdsManager>();
			if(a!= null && a.Length > 1)
			{
				foreach(var ad in a)
				{
					if(ad != this)
					{
						Destroy(ad.gameObject);
					}
				}

				return;
			}


			#if ENABLE_ADMOB
			AddAdmob();
			banner = GetComponent<AAAdmob>();
			#endif

			#if UNITY_ADS
			AddUnityAds();
			#endif

			#if ENABLE_ADCOLONY
			AddADColony();
			#endif

			#if CHARTBOOST
			AddChartboost();
			#endif

			if(interstitialNetworks != null)
			{
				if(listInterstitials == null)
					listInterstitials = new List<IInterstitial>();

				foreach(var m in interstitialNetworks)
				{
					#if ENABLE_ADMOB
					if(m == InterstitialNetwork.Admob)
					{
						if(!listInterstitials.Contains(GetComponent<AAAdmob>()))
						{
							listInterstitials.Add(GetComponent<AAAdmob>());
						}
					}
					#endif

					#if CHARTBOOST
					if(m == InterstitialNetwork.Chartboost)
					{
						if(!listInterstitials.Contains(GetComponent<AAChartboost>()))
						{
							listInterstitials.Add(GetComponent<AAChartboost>());
						}
					}
					#endif
				}
			}


			if(videoNetworks != null)
			{
				if(listVideos == null)
					listVideos = new List<IVideoAds>();

				foreach(var m in videoNetworks)
				{

					#if ENABLE_ADCOLONY
					if(m == VideoNetwork.ADColony)
					{
						if(!listVideos.Contains(GetComponent<AAADColony>()))
						{
							listVideos.Add(GetComponent<AAADColony>());
						}
					}
					#endif

					#if UNITY_ADS
					if(m == VideoNetwork.UnityAds)
					{
						if(!listVideos.Contains(GetComponent<AAUnityAds>()))
						{
							listVideos.Add(GetComponent<AAUnityAds>());
						}
					}
					#endif

				}
			}

			if(rewardedVideoNetworks != null)
			{
				if(listRewardedVideos == null)
					listRewardedVideos = new List<IRewardedVideo>();

				foreach(var m in rewardedVideoNetworks)
				{

					#if ENABLE_ADCOLONY
					if(m == RewardedVideoNetwork.ADColony)
					{
						if(!listRewardedVideos.Contains(GetComponent<AAADColony>()))
						{
							listRewardedVideos.Add(GetComponent<AAADColony>());
						}

					}
					#endif

					#if UNITY_ADS
					if(m == RewardedVideoNetwork.UnityAds)
					{
						if(!listRewardedVideos.Contains(GetComponent<AAUnityAds>()))
						{
							listRewardedVideos.Add(GetComponent<AAUnityAds>());
						}
					}
					#endif

					#if CHARTBOOST
					if(m == RewardedVideoNetwork.Chartboost)
					{
						if(!listRewardedVideos.Contains(GetComponent<AAChartboost>()))
						{
							listRewardedVideos.Add(GetComponent<AAChartboost>());
						}
					}
					#endif

				}
			}


			DontDestroyOnLoad(gameObject);
		}

		IEnumerator Start()
		{
			yield return new WaitForSeconds(1f);
			CacheAllInterstitial();
			yield return new WaitForSeconds(0.2f);
			CacheAllVideoAds();
			yield return new WaitForSeconds(0.2f);
			CacheAllRewardedVideo();

			yield return new WaitForSeconds(1f);

	

			if(bannerNetwork != BannerNetwork.NULL)
			{
				#if ENABLE_ADMOB
				if(bannerNetwork == BannerNetwork.Admob)
				{
					GetComponent<AAAdmob>().ShowBanner();
				}
				#endif
			}

			yield return new WaitForSeconds(2f);

			if(adIds.ShowIntertitialAtStart)
			{
				this.ShowInterstitialStartup();
			}
		}

		public void ShowBanner()
		{
			banner.ShowBanner();
		}

		public void DestroyBanner()
		{
			banner.DestroyBanner();
		}

		public void ShowInterstitialStartup()
		{

			StopAllCoroutines();

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				List<IInterstitial> listTemp = new List<IInterstitial>();

				foreach(var it in listInterstitials)
				{
					listTemp.Add(it);
					print("ShowInterstitialStartup - add : " + it.GetType().ToString());
				}

//				int rand = UnityEngine.Random.Range(0,listTemp.Count);
//				var i = listTemp[rand];
//
//				print("ShowInterstitialStartup - listTemp.Count = " + listTemp.Count + " rand : " + rand.ToString());
//
//				foreach(var itt in listTemp)
//				{
//					itt.CacheInterstitialStartup();
//					itt.CacheInterstitial();
//				}
//					
//				i.ShowInterstitialStartup();
//

				StartCoroutine(_ShowInterstitialStartup(listTemp));
			}
		}

		private IEnumerator _ShowInterstitialStartup(List<IInterstitial> listTemp)
		{
			foreach(var itt in listTemp)
			{
				if(!itt.IsReadyInterstitialStartup())
					itt.CacheInterstitialStartup();

				if(!itt.IsReadyInterstitial())
					itt.CacheInterstitial();
			}

			yield return new WaitForSeconds(1);

			int rand = 0;
			var i = listTemp[rand];

			while(true)
			{
				bool waitForAdsCallBack = true;

				bool breakIt = false;

				print("ShowInterstitialStartup - listTemp.Count = " + listTemp.Count + " rand : " + rand.ToString());


				i.ShowInterstitialStartup((bool success) => {
					if(success)
					{
						breakIt = true;
						waitForAdsCallBack = false;
						StopCoroutine("_ShowInterstitialStartup");
						StopCoroutine(_ShowInterstitialStartup(listTemp));

					}
					else
					{
						breakIt = false;
						waitForAdsCallBack = false;
					}
				});

				while(waitForAdsCallBack)
				{
					yield return new WaitForSeconds(1);
				}

				yield return new WaitForSeconds(1);

				if(breakIt)
					break;
				else
					rand++;

				if(rand >= listTemp.Count)
					break;
			}
		}

		public void ShowInterstitial()
		{
			StopAllCoroutines();

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				List<IInterstitial> listTemp = new List<IInterstitial>();

				foreach(var it in listInterstitials)
				{
					listTemp.Add(it);
					print("ShowInterstitial - add : " + it.GetType().ToString());
				}

//				int rand = UnityEngine.Random.Range(0,listTemp.Count);
//				var i = listTemp[rand];
//
//				print("ShowInterstitial - listTemp.Count = " + listTemp.Count + " rand : " + rand.ToString());
//
//				foreach(var itt in listTemp)
//				{
//					itt.CacheInterstitial();
//				}
//
//				if(!i.IsReadyInterstitial())
//				{
//					print("ShowInterstitial - !i.IsReadyInterstitial()");
//
//					i.CacheInterstitial();
//
//					listTemp.RemoveAt(rand);
//
//					foreach(var itt in listTemp)
//					{
//						if(itt.IsReadyInterstitial())
//						{
//							itt.ShowInterstitial();
//							return;
//						}
//						else
//						{
//							itt.CacheInterstitial();
//						}
//					}
//				}
//				else
//				{
//					i.ShowInterstitial();
//					return;
//				}

				StartCoroutine(_ShowInterstitial(listTemp));
			}
		}

		private IEnumerator _ShowInterstitial(List<IInterstitial> listTemp)
		{
			foreach(var itt in listTemp)
			{
				if(!itt.IsReadyInterstitial())
					itt.CacheInterstitial();
			}

			yield return new WaitForSeconds(1);

			int rand = 0;
			var i = listTemp[rand];

			while(true)
			{
				bool waitForAdsCallBack = true;

				bool breakIt = false;

				print("ShowInterstitial - listTemp.Count = " + listTemp.Count + " rand : " + rand.ToString());


				i.ShowInterstitial((bool success) => {
					if(success)
					{
						breakIt = true;
						waitForAdsCallBack = false;
						StopCoroutine("_ShowInterstitial");
						StopCoroutine(_ShowInterstitial(listTemp));

					}
					else
					{
						breakIt = false;
						waitForAdsCallBack = false;
					}
				});

				while(waitForAdsCallBack)
				{
					yield return new WaitForSeconds(1);
				}

				yield return new WaitForSeconds(1);

				if(breakIt)
					break;
				else
					rand++;

				if(rand >= listTemp.Count)
					break;
			}
		}

		void CacheAllInterstitial()
		{
			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				foreach(var itt in listInterstitials)
				{
					itt.CacheInterstitial();
				}
			}
		}

		public bool IsReadyInterstitial()
		{
			bool isReady = false;

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				foreach(var itt in listInterstitials)
				{
					if(itt.IsReadyInterstitial())
					{
						isReady = true;
					}
//					else
//					{
//						itt.CacheInterstitial();
//					}
				}
			}

			return isReady;
		}

		public void ShowVideoAds()
		{
			if(listVideos != null && listVideos.Count > 0)
			{
				List<IVideoAds> listTemp = new List<IVideoAds>();

				foreach(var it in listVideos)
				{
					listTemp.Add(it);
				}

				int rand = UnityEngine.Random.Range(0,listTemp.Count);
				var i = listTemp[rand];

				if(!i.IsReadyVideoAds())
				{
					i.CacheVideoAds();

					listTemp.RemoveAt(rand);

					foreach(var itt in listTemp)
					{
						if(itt.IsReadyVideoAds())
						{
							itt.ShowVideoAds();
							return;
						}
						else
						{
							itt.CacheVideoAds();
						}
					}
				}
				else
				{
					i.ShowVideoAds();
					return;
				}
			}
		}

		void CacheAllVideoAds()
		{

			if(listVideos != null && listVideos.Count > 0)
			{
				foreach(var itt in listVideos)
				{
					
					itt.CacheVideoAds();
				}
			}
		}

		public bool IsReadyVideoAds()
		{
			bool isReady = false;

			if(listVideos != null && listVideos.Count > 0)
			{
				foreach(var itt in listVideos)
				{
					if(itt.IsReadyVideoAds())
					{
						isReady = true;
					}
//					else
//					{
//						itt.CacheVideoAds();
//					}
				}
			}

			return isReady;
		}

		public void ShowRewardedVideo(Action<bool> success)
		{
			if(listRewardedVideos != null && listRewardedVideos.Count > 0)
			{
				List<IRewardedVideo> listTemp = new List<IRewardedVideo>();

				foreach(var it in listRewardedVideos)
				{
					listTemp.Add(it);
				}

				int rand = UnityEngine.Random.Range(0,listTemp.Count);
				var i = listTemp[rand];

				if(!i.IsReadyRewardedVideo())
				{
					i.CacheRewardedVideo();

					listTemp.RemoveAt(rand);

					foreach(var itt in listTemp)
					{
						if(itt.IsReadyRewardedVideo())
						{
							itt.ShowRewardedVideo(success);
							return;
						}
						else
						{
							itt.CacheRewardedVideo();
						}
					}
				}
				else
				{
					i.ShowRewardedVideo(success);
				}
			}

			if(success != null)
			{
				success(false);
			}
		}

		void CacheAllRewardedVideo()
		{
			if(listRewardedVideos != null && listRewardedVideos.Count > 0)
			{
				foreach(var itt in listRewardedVideos)
				{
					itt.CacheRewardedVideo();
				}
			}
		}

		public bool IsReadyRewardedVideo()
		{
			bool isReady = false;

			if(listRewardedVideos != null && listRewardedVideos.Count > 0)
			{
				foreach(var itt in listRewardedVideos)
				{
					if(itt.IsReadyRewardedVideo())
					{
						isReady = true;
					}
					else
					{
						itt.CacheRewardedVideo();
					}
				}
			}

			return isReady;
		}
	}
}