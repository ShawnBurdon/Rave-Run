public enum BannerNetwork
{
	#if ENABLE_ADMOB
	Admob,
	#endif

	#if ENABLE_IAD
	iAd,
	#endif

	NULL
}

public enum InterstitialNetwork
{
	#if ENABLE_ADMOB
	Admob,
	#endif

//	#if ENABLE_IAD
//	iAd,
//	#endif

	#if CHARTBOOST
	Chartboost,
	#endif

	NULL
}


public enum RewardedVideoNetwork
{
	#if ENABLE_ADMOB
	Admob,
	#endif

	#if UNITY_ADS
	UnityAds,
	#endif

	#if CHARTBOOST
	Chartboost,
	#endif

	#if ENABLE_ADCOLONY
	ADColony,
	#endif

	NULL
}


public enum VideoNetwork
{
	#if UNITY_ADS
	UnityAds,
	#endif

	#if ENABLE_ADCOLONY
	ADColony,
	#endif

	NULL
}