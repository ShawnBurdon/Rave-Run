using UnityEngine;
using System.Collections;

namespace AppAdvisory.Ads
{
	public interface IVideoAds : IIBase
	{
		bool IsReadyVideoAds();
		void CacheVideoAds();
		void ShowVideoAds();
	}
}