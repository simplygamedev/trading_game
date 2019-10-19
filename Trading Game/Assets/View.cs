using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System;

public class View : MonoBehaviour {

	public StringBuilder sb = new StringBuilder ();

	MarketController mc;

	private string transactionamount;
	private string resource;
	private string selectedMarket = "Luebeck";

	GUIStyle gs = new GUIStyle ();

	Player player = new Player();

	public void Start(){

		gs.normal.textColor = Color.blue;

		mc = GameObject.Find("MarketController").GetComponent<MarketController>();
	}

	public void OnGUI()
	{
		sb.Clear ();
		GUILayout.Label (selectedMarket);
		GUILayout.Label (mc.GetMarketInfoByName(selectedMarket));

		if(GUILayout.Button(String.Format("Buy {0}", transactionamount))){
			int qty;
			string transactResource = String.IsNullOrEmpty (resource) ? "" : resource;
			if(int.TryParse(transactionamount, out qty)){
				int totalPrice = mc.calculatePrice (selectedMarket, transactResource, qty);
				Debug.Log (String.Format("Resource: {0}\nMarket: {1}\nQuantity: {2}\nTotal Price: {3}", transactResource, selectedMarket, qty, totalPrice));
				if(player.BuyResource (transactResource, totalPrice, qty)){
					mc.BuyByMarket (selectedMarket, transactResource, qty);
				}
			}
		}
			
		if(GUILayout.Button(String.Format("Sell {0}", transactionamount))){
			int qty;
			string transactResource = String.IsNullOrEmpty (resource) ? "" : resource;
			if(int.TryParse(transactionamount, out qty)){
				int totalPrice = mc.calculatePrice (selectedMarket, transactResource, qty);
				Debug.Log (String.Format("Resource: {0}\nMarket: {1}\nQuantity: {2} Total Price:\n{3}", transactResource, selectedMarket, qty, totalPrice));
				if(player.SellResources (transactResource, totalPrice, qty)){
					mc.SellByMarket (selectedMarket, transactResource, qty);
				}
			}
		}

		if(GUILayout.Button("Gdansk")){
			selectedMarket = "Gdansk";
		}

		if(GUILayout.Button("Luebeck")){
			selectedMarket = "Luebeck";
		}
			
		transactionamount = GUILayout.TextField (transactionamount);
		resource  = GUILayout.TextField(resource);

		if(GUILayout.Button("Toggle Resource Update")){
			mc.toggleResourceUpdate ();
		}

		GUILayout.Label (player.ShowPlayerInfo(), gs);
	}
}
