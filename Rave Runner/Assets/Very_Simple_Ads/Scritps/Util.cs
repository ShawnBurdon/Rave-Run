using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AppAdvisory.Ads
{
	/// <summary>
	/// Utility class. This class is static, so you can use it in all your projects!
	/// </summary>
	public static class Util
	{
		private static System.Random rng = new System.Random();  
		/// <summary>
		/// Real shuffle of List
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)  
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = rng.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}  
		}
	}
}