using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour
{

	public IAP iap;

	public void DoubleWater ()
	{
		iap.BuyConsumable(IAP.Consumable.DoubleWater);
	}
	
	public void ExtraLives ()
	{
		iap.BuyConsumable(IAP.Consumable.ExtraLife);
	}
}
