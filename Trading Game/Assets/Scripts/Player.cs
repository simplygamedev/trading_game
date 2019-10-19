using System.Collections.Generic;
using System.Text;

public class Player 
{
	public int gold = 100;
	StringBuilder sb = new StringBuilder();

	public Player(){
		string[] resources = Market.stateAvailableResources ();
		foreach(var resource in resources){
			inventories.Add (resource, new Inventory());
		}
	}

	public Dictionary<string, Inventory> inventories = new Dictionary<string, Inventory> ();

	public bool BuyResource(string resourceName, int totalPrice, int totalQty){
		if(gold >= totalPrice && inventories.ContainsKey(resourceName)){
			gold -= totalPrice;
			inventories [resourceName].Buy (totalQty,totalPrice);
			return true;
		}
		return false;
	}

	public bool SellResources(string resourceName, int totalPrice, int totalQty){
		if(inventories.ContainsKey(resourceName) && inventories[resourceName].quantity >= totalQty){
			gold += totalPrice;
			inventories [resourceName].Sell (totalQty,totalPrice);
			return true;
		}
		return false;
	}

	public string ShowPlayerInfo(){
		sb.Clear ();
		sb.AppendFormat ("Gold: {0}\n", gold);
		foreach(var inventory in inventories){
			sb.AppendFormat ("{0} On Hand: {1} {0} Average Price: {2}\n", inventory.Key, inventory.Value.quantity, inventory.Value.calculateAveragePrice());
		}
		return sb.ToString ();
	}
}
