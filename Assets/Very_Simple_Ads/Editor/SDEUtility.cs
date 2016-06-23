using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace AppAdvisory.Ads
{
	public static class SDEUtility 
	{
		const string menuPath = "GameObject/";

		//	public static void CreateAsset<T>(string name) where T : ScriptableObject
		//	{
		//		var asset = ScriptableObject.CreateInstance<T>();
		//		ProjectWindowUtil.CreateAsset(asset, name + ".asset");
		//	}
		//
		//	[MenuItem("Assets/Create/ADS_SETTING")]
		//	public static void CreateAdIds()
		//	{
		//		CreateAsset<ADIDS>("ADS_SETTING");
		//	}

		[MenuItem ( menuPath + "APP ADVISORY/Very Simple Ads/CREATE AdsInit",false,20)]
		public static void CreateAdInits()
		{
			GameObject gameObject = new GameObject("AdsInit");
			AdsInit a = gameObject.AddComponent<AdsInit>();
			string[] guids = AssetDatabase.FindAssets("ADS_SETTING");

			#if UNITY_5_3
			a.adIdds =  AssetDatabase.LoadAssetAtPath<ADIDS>( AssetDatabase.GUIDToAssetPath( guids[0] ));
			a.SetADIDS(AssetDatabase.LoadAssetAtPath<ADIDS>( AssetDatabase.GUIDToAssetPath( guids[0] )));
			#else
			a.adIdds =  AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(ADIDS)) as ADIDS;
			a.SetADIDS(AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( guids[0] ), typeof(ADIDS))  as ADIDS );
			#endif
			//			Autoselect();
		}

		[MenuItem("Tools/APP ADVISORY/Very Simple Ads/OPEN ADS SETTINGS", false, 0)]
		[MenuItem("Window/APP ADVISORY/Very Simple Ads/OPEN ADS SETTINGS", false, 0)]
		public static void Autoselect()
		{
			string[] guids = AssetDatabase.FindAssets("ADS_SETTING");
			#if UNITY_5_3
			Selection.activeObject = AssetDatabase.LoadAssetAtPath<ADIDS>( AssetDatabase.GUIDToAssetPath( guids[0] ));
			#else
			Selection.activeObject = AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( guids[0] ), typeof(ADIDS)) ;
			#endif
		}
	}
}