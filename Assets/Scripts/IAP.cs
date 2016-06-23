using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAP : MonoBehaviour, IStoreListener//, IStoreListener
{
	public Store store;

	private static IStoreController m_StoreController;                                                                  // Reference to the Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;                                                         // Reference to store-specific Purchasing subsystems.

	// Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts 
	// for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

	private static string IDsuperOne = "superseedsPackOne";                                                        // General handle for the consumable product.
	private static string IDsuperTwo = "superseedsPackTwo";                                                       // General handle for the consumable product.
	private static string IDsuperThree = "superseedsPackThree";                                                  // General handle for the consumable product.
	private static string IDsuperFour = "superseedsPackFour";                                                   // General handle for the consumable product.

	private static string IDgoldenOne = "goldenPackOne";                                                        // General handle for the consumable product.
	private static string IDgoldenTwo = "goldenPackTwo";                                                       // General handle for the consumable product.
	private static string IDgoldenThree = "goldenPackThree";                                                  // General handle for the consumable product.
	private static string IDgoldenFour = "goldenPackFour";

	//Apple Superseeds
	private static string AppleSuperseedOne = "superseedsPackOne";                 // Apple App Store identifier for the consumable product.
	private static string AppleSuperseedTwo = "superseedsPackTwo";                // Apple App Store identifier for the consumable product.
	private static string AppleSuperseedThree = "superseedsPackThree";             // Apple App Store identifier for the consumable product.
	private static string AppleSuperseedFour = "superseedsPackFour";             // Apple App Store identifier for the consumable product.
																				 //Apple Golden
	private static string AppleGoldenOne = "goldenApplePackOne";                 // Apple App Store identifier for the consumable product.
	private static string AppleGoldenTwo = "goldenApplePackTwo";                // Apple App Store identifier for the consumable product.
	private static string AppleGoldenThree = "goldenApplePackThree";             // Apple App Store identifier for the consumable product.
	private static string AppleGoldenFour = "goldenApplePackFour";             // Apple App Store identifier for the consumable product.
																			   //Google Superseed
	private static string GoogleSuperseedOne = "goldenApplePackOne";             // Google Play Store identifier for the consumable product.
	private static string GoogleSuperseedTwo = "goldenApplePackOne";            // Google Play Store identifier for the consumable product.
	private static string GoogleSuperseedThree = "goldenApplePackOne";         // Google Play Store identifier for the consumable product.
	private static string GoogleSuperseedFour = "goldenApplePackOne";         // Google Play Store identifier for the consumable product.
																			  //Google Golden																							
	private static string GoogleGoldenOne = "goldenApplePackOne";             // Google Play Store identifier for the consumable product.
	private static string GoogleGoldenTwo = "goldenApplePackOne";            // Google Play Store identifier for the consumable product.
	private static string GoogleGoldenThree = "goldenApplePackOne";         // Google Play Store identifier for the consumable product.
	private static string GoogleGoldenFour = "goldenApplePackOne";         // Google Play Store identifier for the consumable product.

	[HideInInspector]
	public Consumable consumable;

	public enum Consumable
	{
		SuperOne,
		SuperTwo,
		SuperThree,
		SuperFour,
		GoldenOne,
		GoldenTwo,
		GoldenThree,
		GoldenFour
	}

	//void Start()
	//{
	//	// If we haven't set up the Unity Purchasing reference
	//	if (m_StoreController == null)
	//	{
	//		// Begin to configure our connection to Purchasing
	//		InitializePurchasing();
	//	}
	//}

	//public void InitializePurchasing()
	//{
	//	// If we have already connected to Purchasing ...
	//	if (IsInitialized())
	//	{
	//		// ... we are done here.
	//		return;
	//	}

	//	// Create a builder, first passing in a suite of Unity provided stores.
	//	var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

	//	// Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
	//	//===========================Superseeds=====================
	//	builder.AddProduct(IDsuperOne, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleSuperseedOne, AppleAppStore.Name },
	//		{ GoogleSuperseedOne, GooglePlay.Name },
	//	});

	//	builder.AddProduct(IDsuperTwo, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleSuperseedTwo, AppleAppStore.Name },
	//		{ GoogleSuperseedTwo, GooglePlay.Name },
	//	});

	//	builder.AddProduct(IDsuperThree, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleSuperseedThree, AppleAppStore.Name },
	//		{ GoogleSuperseedThree, GooglePlay.Name },
	//	});

	//	builder.AddProduct(IDsuperFour, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleSuperseedFour, AppleAppStore.Name },
	//		{ GoogleSuperseedFour, GooglePlay.Name },
	//	});

	//	//===========================Golden=====================

	//	builder.AddProduct(IDgoldenOne, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleGoldenOne, AppleAppStore.Name },
	//		{ GoogleGoldenOne, GooglePlay.Name },
	//	});

	//	builder.AddProduct(IDgoldenTwo, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleGoldenTwo, AppleAppStore.Name },
	//		{ GoogleGoldenTwo, GooglePlay.Name },
	//	});

	//	builder.AddProduct(IDgoldenThree, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleGoldenThree, AppleAppStore.Name },
	//		{ GoogleGoldenThree, GooglePlay.Name },
	//	});

	//	builder.AddProduct(IDgoldenFour, ProductType.Consumable, new IDs()
	//	{
	//		{ AppleGoldenFour, AppleAppStore.Name },
	//		{ GoogleGoldenFour, GooglePlay.Name },
	//	});

	//	UnityPurchasing.Initialize(this, builder);
	//}


	//private bool IsInitialized()
	//{
	//	// Only say we are initialized if both the Purchasing references are set.
	//	return m_StoreController != null && m_StoreExtensionProvider != null;
	//}


	//public void BuyConsumable(Consumable item)
	//{
	//	// Buy the consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.

	//	switch (item)
	//	{
	//		case Consumable.SuperOne:
	//			BuyProductID(IDsuperOne);
	//			break;
	//		case Consumable.SuperTwo:
	//			BuyProductID(IDsuperTwo);
	//			break;
	//		case Consumable.SuperThree:
	//			BuyProductID(IDsuperThree);
	//			break;
	//		case Consumable.SuperFour:
	//			BuyProductID(IDsuperFour);
	//			break;
	//		case Consumable.GoldenOne:
	//			BuyProductID(IDgoldenOne);
	//			break;
	//		case Consumable.GoldenTwo:
	//			BuyProductID(IDgoldenTwo);
	//			break;
	//		case Consumable.GoldenThree:
	//			BuyProductID(IDgoldenThree);
	//			break;
	//		case Consumable.GoldenFour:
	//			BuyProductID(IDgoldenFour);
	//			break;
	//		default:
	//			break;
	//	}
	//}



	//void BuyProductID(string productId)
	//{
	//	// If the stores throw an unexpected exception, use try..catch to protect my logic here.
	//	try
	//	{
	//		// If Purchasing has been initialized ...
	//		if (IsInitialized())
	//		{
	//			// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
	//			Product product = m_StoreController.products.WithID(productId);

	//			// If the look up found a product for this device's store and that product is ready to be sold ... 
	//			if (product != null && product.availableToPurchase)
	//			{
	//				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
	//				m_StoreController.InitiatePurchase(product);
	//			}
	//			// Otherwise ...
	//			else
	//			{
	//				// ... report the product look-up failure situation  
	//				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
	//			}
	//		}
	//		// Otherwise ...
	//		else
	//		{
	//			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
	//			Debug.Log("BuyProductID FAIL. Not initialized.");
	//		}
	//	}
	//	// Complete the unexpected exception handling ...
	//	catch (Exception e)
	//	{
	//		// ... by reporting any unexpected exception for later diagnosis.
	//		Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
	//	}
	//}


	//// Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
	//public void RestorePurchases()
	//{
	//	// If Purchasing has not yet been set up ...
	//	if (!IsInitialized())
	//	{
	//		// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
	//		Debug.Log("RestorePurchases FAIL. Not initialized.");
	//		return;
	//	}

	//	// If we are running on an Apple device ... 
	//	if (Application.platform == RuntimePlatform.IPhonePlayer ||
	//		Application.platform == RuntimePlatform.OSXPlayer)
	//	{
	//		// ... begin restoring purchases
	//		Debug.Log("RestorePurchases started ...");

	//		// Fetch the Apple store-specific subsystem.
	//		var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
	//		// Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
	//		apple.RestoreTransactions((result) => {
	//			// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
	//			Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
	//		});
	//	}
	//	// Otherwise ...
	//	else
	//	{
	//		// We are not running on an Apple device. No work is necessary to restore purchases.
	//		Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
	//	}
	//}


	////  
	//// --- IStoreListener
	////

	//public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	//{
	//	// Purchasing has succeeded initializing. Collect our Purchasing references.
	//	Debug.Log("OnInitialized: PASS");

	//	// Overall Purchasing system, configured with products for this application.
	//	m_StoreController = controller;
	//	// Store specific subsystem, for accessing device-specific store features.
	//	m_StoreExtensionProvider = extensions;
	//}


	//public void OnInitializeFailed(InitializationFailureReason error)
	//{
	//	// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
	//	Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	//}


	//public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	//{
	//	// A consumable product has been purchased by this user.
	//	if (String.Equals(args.purchasedProduct.definition.id, IDsuperOne, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(0);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDsuperTwo, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(1);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDsuperThree, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(2);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDsuperFour, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(3);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDgoldenOne, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(4);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDgoldenTwo, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(5);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDgoldenThree, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(6);
	//	}
	//	else if (String.Equals(args.purchasedProduct.definition.id, IDgoldenFour, StringComparison.Ordinal))
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
	//		store.PurchaseComplete(7);
	//	}
	//	else
	//	{
	//		Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
	//	}// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.
	//	return PurchaseProcessingResult.Complete;
	//}


	//public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	//{
	//	// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
	//	Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	//}
}