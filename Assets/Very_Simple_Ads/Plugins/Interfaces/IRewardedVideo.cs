using UnityEngine;
using System;
using System.Collections;

namespace AppAdvisory.Ads
{
	public interface IRewardedVideo : IIBase
	{
		bool IsReadyRewardedVideo();
		void CacheRewardedVideo();
		void ShowRewardedVideo(Action<bool> success);
	}
}