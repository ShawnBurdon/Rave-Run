#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used


using UnityEngine;
using System.Collections;
using System;

namespace AppAdvisory.Ads
{
	public class AdBase : MonoBehaviour
	{
		public ADIDS adIds
		{
			get
			{
				return FindObjectOfType<AppAdvisory.Ads.AdsManager>().adIds;
			}
		}
	}
}