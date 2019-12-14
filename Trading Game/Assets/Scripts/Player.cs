using System.Collections.Generic;
using System.Text;

public class Player 
{
	public int gold = 100;
	StringBuilder sb = new StringBuilder();

	public Player()
    {
        foreach (var resource in Market.resourceNames)
        {
            inventories.Add(resource, new Inventory());
        }
	}

	public Dictionary<string, Inventory> inventories = new Dictionary<string, Inventory> ();

	public bool BuyResource(string resourceName, int totalPrice, int totalQty){
        if(gold>=totalPrice && inventories.ContainsKey(resourceName))
        {
            gold -= totalPrice;
            inventories[resourceName].Buy(totalQty, totalPrice);
            return true;
        }
        return false;
	}

	public bool SellResources(string resourceName, int totalPrice, int totalQty){
        if (inventories.ContainsKey(resourceName) && inventories[resourceName].quantity >=totalQty)
        {
            gold += totalPrice;
            inventories[resourceName].Sell(totalQty, totalPrice);
            return true;
        }
        return false;
	}

	public string ShowPlayerInfo(){
        sb.Clear();
        foreach (var item in inventories)
        {
            sb.AppendFormat("Resource Name:{0}, Resource Quantity:{1}\n", item.Key, item.Value);
        }
        return sb.ToString();
	}
}
