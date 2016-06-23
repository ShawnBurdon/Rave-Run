using UnityEngine;
using System;
using System.Collections;

namespace AppAdvisory.Ads
{
	public interface IInterstitial : IIBase
	{
		bool IsReadyInterstitial();
		bool IsReadyInterstitialStartup();
		void CacheInterstitial();
		void CacheInterstitialStartup();
		void ShowInterstitial(Action<bool> succes);
		void ShowInterstitialStartup(Action<bool> succes);
	}
}