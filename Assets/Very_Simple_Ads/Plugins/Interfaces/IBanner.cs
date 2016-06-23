using UnityEngine;
using System.Collections;

namespace AppAdvisory.Ads
{
	public interface IBanner : IIBase
	{
		void ShowBanner();
		void HideBanner();
		void DestroyBanner();
	}
}