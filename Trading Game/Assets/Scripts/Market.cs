using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


/// <summary>
/// Resource Market that handles buying and selling of resources
/// It inherits from MonoBehaviour so that we can use coroutines to automate resource quantity change in this class.
/// </summary>
public class Market : MonoBehaviour {

    public static Dictionary<string, Market> markets
    {
        get;
        private set;
    }

	private Dictionary<string, Resources> resourceList = new Dictionary<string, Resources>();

    public static List<string> resourceNames = new List<string>();

    public void Awake()
    {
        if (markets == null)
            markets = new Dictionary<string, Market>();

        string resourcesInJson = HandleInputFile.ReadString("InputFiles/Resources", $"{name.ToLowerInvariant()}_resource.json");
        List<SerializedResource> resources = JsonConvert.DeserializeObject<List<SerializedResource>>(resourcesInJson);

        markets.Add(name, this);

        foreach (SerializedResource resource in resources)
        {
            int initialAmount = resource.InitialAmount;
            float equilibrium_price = resource.EquilibriumPrice;
            float equilibrium_qty = resource.EquilibriumQty;
            float price_elasticity = resource.PriceElasticity;

            if (!resourceNames.Contains(resource.Name))
                resourceNames.Add(resource.Name);

            resourceList.Add(resource.Name, Resources.createNewResourceByName(resource.Name, new float[] { initialAmount, equilibrium_price, equilibrium_qty, price_elasticity, 0, 0 }));
        }
    }

    /// <summary>
    /// Get the price of the stated resource
    /// </summary>
    /// <param name="resource">name of the resource</param>
    /// <param name="qty">quantity of the resource to calculate the price of</param>
    /// <returns>
    /// Returns the total price for the stated quantity of resource
    /// returns -1 if the resource is not found, 
    /// returns -2 if the calculateTotalPrice() operation returns 0
    /// </returns>
    public float getPrice(string resource, int qty)
    {
        if (resourceList.ContainsKey(resource))
        {
            float price = resourceList[resource].calculateTotalPrice(qty);
            return (price > 0) ? price : -2; 
        }

        return -1;
    }

    /// <summary>
    /// Get the total quanitity of the stated resource
    /// </summary>
    /// <param name="resource">name of the resource</param>
    /// <returns>Returns the total quantity of the stated resource, returns -1 if the resource is not found</returns>
    public int getQuantity(string resource)
    {
        return (resourceList.ContainsKey(resource)) ? resourceList[resource].getState(): -1;
    }

    /// <summary>
    /// Decrease the Quantity of the specified resource
    /// </summary>
    /// <param name="resource">name of the resource to update the quantity of</param>
    /// <param name="quantity">amount by which to update the stated resource</param>
    /// <returns>Returns the status in boolean of whether the Resource operation was successful or not</returns>
    public bool DecreaseQuantity(string resource, int quantity)
	{
           if (resourceList.ContainsKey(resource)) {
                int currentQuantity = resourceList[resource].getState();

                if (currentQuantity > 0 && currentQuantity - quantity >= 0)
                    return resourceList[resource].DecreaseQuantity(quantity);
           }

	    return false;
	}

    /// <summary>
    /// Increase the Quantity of the specified resource
    /// </summary>
    /// <param name="resource">name of the resource to update the quantity of</param>
    /// <param name="quantity">amount by which to update the stated resource</param>
    /// <returns>Returns the status in boolean of whether the Resource operation was successful or not</returns>
    public bool IncreaseQuantity(string resource, int quantity)
	{
        return (resourceList.ContainsKey(resource)) ? resourceList[resource].IncreaseQuantity(quantity) : false;
	}

    public Dictionary<string, Resources> GetResourceInfo()
    {
        return resourceList;
    }
}