using System.Collections.Generic;
using UnityEngine;
using System;


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

	public int GrainInitialAmount = 30;
    public int GrainConsumption = 3;
    public int GrainProduction = 1;
    public const float GrainEquilibriumPrice = 35;
    public const float GrainEquilibriumQty = 60;
    public const float GrainPriceElasticity = -1;

    public int BeerInitialAmount = 40;
    public int BeerConsumption = 5;
    public int BeerProduction = 1;
    public const float BeerEquilibriumPrice = 50f;
    public const float BeerEquilibriumQty = 60;
    public const float BeerPriceElasticity = -1;

    public int IronInitialAmount = 30;
    public int IronConsumption = 3;
    public int IronProduction = 1;
    public const float IronEquilibriumPrice = 100f;
    public const float IronEquilibriumQty = 60;
    public const float IronPriceElasticity = -0.01f;

    public static readonly string[] resourceNames = {"Beer", "Iron", "Grain"};

    public void Awake()
    {
        if (markets == null)
            markets = new Dictionary<string, Market>();

        markets.Add(name, this);

        foreach (var name in resourceNames)
        {
            int initialAmount = GetAttribute<int>(string.Format("{0}InitialAmount", name));
            float equilibrium_price = GetAttribute<float>(string.Format("{0}EquilibriumPrice", name));
            float equilibrium_qty = GetAttribute<float>(string.Format("{0}EquilibriumQty", name));
            float price_elasticity = GetAttribute<float>(string.Format("{0}PriceElasticity", name));
            resourceList.Add(name, Resources.createNewResourceByName(name, new float[]{ initialAmount, equilibrium_price, equilibrium_qty, price_elasticity, 0, 0}));
        }
    }

    /// <summary>
    /// used during initialization to get the field values for initializing the respective resources via reflection
    /// </summary>
    /// <typeparam name="T">market objects field type that we wish to return</typeparam>
    /// <param name="_name">name of the field we wish to return</param>
    /// <returns>value of the specified field</returns>
    private T GetAttribute<T>(string _name)
    {
        
        return (T)GetType().GetField(_name).GetValue(this);
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