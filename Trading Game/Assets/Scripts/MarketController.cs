using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class MarketController : MonoBehaviour {

	public List<GameObject> marketGameObjects = new List<GameObject>();

	Dictionary<string, Market> markets = new Dictionary<string, Market>();

	public StringBuilder sb = new StringBuilder ();

	public float resourceUpdateSeconds = 5f;

	void Start () 
	{
		foreach (var marketobject in marketGameObjects) {
			markets.Add (marketobject.name, marketobject.GetComponent<Market>());
		}
	}

	public string GetMarketInfoByName(string marketName){

		sb.Clear ();

		if (marketExist (marketName)) 
		{
			foreach (var resource in markets [marketName].resourceList) 
			{
				sb.AppendFormat ("{0} Amount: {1} {0} Price: {2}\n", resource.Key, resource.Value.getState (), resource.Value.calculatePrice ());
			}

			return sb.ToString ();

		} 

		return "Market Does Not Exist";
	}

	public bool BuyByMarket(string marketName, string resourceName, int qty){
		if (marketExist (marketName)) {
			if(markets [marketName].DecreaseQuantity (resourceName, qty)){
				return true;
			}
		}
		return false;
	}

	public bool SellByMarket(string marketName, string resourceName, int qty){
		if (marketExist (marketName)) {
			if(markets [marketName].IncreaseQuantity (resourceName, qty)){
				return true;
			}
		}
		return false;
	}

	public int calculatePrice(string marketName, string resourceName, int qty){
		if(marketExist(marketName)){
			if(markets[marketName].resourceList.ContainsKey(resourceName)){
				return markets [marketName].resourceList [resourceName].calculateTotalPrice (qty); 
			}
		}

		return 0;
	}

	private bool marketExist(string marketName){
		return markets.ContainsKey (String.IsNullOrEmpty(marketName) ? "" : marketName);
	}

	public void toggleResourceUpdate(){
		foreach (var marketName in markets.Keys) {
			markets [marketName].startResourceUpdate (resourceUpdateSeconds);
		}
	}
}
