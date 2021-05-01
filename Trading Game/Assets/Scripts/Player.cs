using System.Collections.Generic;
using System.Text;

public class Player 
{
	public int gold = 100;
	StringBuilder sb = new StringBuilder();
    public Dictionary<string, Inventory> inventories = new Dictionary<string, Inventory>();

	public bool BuyResource(string resourceName, int totalPrice, int totalQty){
        checkHasInventoryItem(resourceName);
        if (gold>=totalPrice && inventories.ContainsKey(resourceName))
        {
            gold -= totalPrice;
            inventories[resourceName].Buy(totalQty, totalPrice);
            return true;
        }
        return false;
	}

    private void checkHasInventoryItem(string resourceName)
    {
        if(!inventories.ContainsKey(resourceName))
            inventories.Add(resourceName, new Inventory());
    }

    /// <summary>
    /// Sell resources specified by resourceName, 
    /// Total gold to earn from the sell specified by totalPrice,
    /// and total quantity to sell specified by totalQty
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="totalPrice"></param>
    /// <param name="totalQty"></param>
    /// <returns>Returns a boolean to show if the attempt to sell the resource was successful or not</returns>
	public bool SellResources(string resourceName, int totalPrice, int totalQty){
        if (inventories.ContainsKey(resourceName) && inventories[resourceName].quantity >=totalQty)
        {
            gold += totalPrice;
            inventories[resourceName].Sell(totalQty, totalPrice);
            return true;
        }
        return false;
	}

	public string getPlayerInfo(){
        sb.Clear();
        foreach (var item in inventories)
        {
            sb.AppendFormat("Resource Name:{0}, Resource Quantity:{1}\n", item.Key, item.Value);
        }
        return sb.ToString();
	}
}
