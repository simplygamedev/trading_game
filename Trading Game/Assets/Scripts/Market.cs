using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Resource Market that handles buying and selling of resources
/// It inherits from MonoBehaviour so that we can use coroutines to automate resource quantity change in this class.
/// </summary>
/// TODO - We should create another Class or Struct which defines a specific Markets resource quantity to fluctuate by 
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

    static System.Random rand = new System.Random();

    System.Collections.IEnumerator randomizeResourceFluctuation;

    public void Awake()
    {
        if (markets == null)
            markets = new Dictionary<string, Market>();

        markets.Add(name, this);

        foreach (var name in resourceNames)
        {
            InitializeResourceByName(name);
        }
    }

    public void InitializeResourceByName(String name)
    {
        int initialAmount = GetAttribute<int>(string.Format("{0}InitialAmount", name));
        float equilibrium_price = GetAttribute<float>(string.Format("{0}EquilibriumPrice", name));
        float equilibrium_qty = GetAttribute<float>(string.Format("{0}EquilibriumQty", name));
        float price_elasticity = GetAttribute<float>(string.Format("{0}PriceElasticity", name));
        resourceList.Add(name, Resources.createNewResourceByName(name, new float[] { initialAmount, equilibrium_price, equilibrium_qty, price_elasticity, 0, 0 }));
    }

    public void Start()
    {
        StartResourceFluctuation();
    }

    /// <summary>
    /// Get Class field values via reflection
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
    /// Returns -1 if the resource is not found, 
    /// Returns -2 if the calculateTotalPrice() operation returns 0
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
    public void DecreaseQuantity(string resource, int quantity)
	{
           if (resourceList.ContainsKey(resource)) {
                int currentQuantity = resourceList[resource].getState();

                if (currentQuantity > 0 && currentQuantity - quantity >= 0)
                    resourceList[resource].DecreaseQuantity(quantity);
           }
	}

    /// <summary>
    /// Increase the Quantity of the specified resource
    /// </summary>
    /// <param name="resource">name of the resource to update the quantity of</param>
    /// <param name="quantity">amount by which to update the stated resource</param>
    public void IncreaseQuantity(string resource, int quantity)
	{
        if(resourceList.ContainsKey(resource))
            resourceList[resource].IncreaseQuantity(quantity);
	}

    /// <summary>
    /// Returns a dictionary containing Full Resource Inforamtion for this Market
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, Resources> GetResourceInfo()
    {
        return resourceList;
    }

    /// <summary>
    /// IEnumerator for beginning Random Market Influx and Outflux of Resources
    /// </summary>
    private System.Collections.IEnumerator IRandomizeResourceFluctuation()
    {
        while (true)
        {
            // Randomize resource fluctuation between 5 to 15 seconds
            yield return new WaitForSeconds(rand.Next(5, 15));

            //Randomize the Amount to Update a Resources Quantity by
            int randomQuantityUpdate = rand.Next(0, 4);

            //Select a Random Resource to fluctuate its quantity
            string selectedResource = resourceNames[rand.Next(0, resourceNames.Length)];

            //Select between 1=IncreaseQuantity & 0=DecreaseQuantity
            if (rand.Next(0, 2) == 1)
                IncreaseQuantity(selectedResource, randomQuantityUpdate);
            else
                DecreaseQuantity(selectedResource, randomQuantityUpdate);
        }
    }

    /// <summary>
    /// IEnumerator for randomizing increasing the resource specified by parameter name, 
    /// at a rate specified by parameter timeQuantum and amount specified by parameter amount
    /// </summary>
    /// <param name="name">Name of the resource to increase amount</param>
    /// <param name="amount">Random maximum quantity to increase the resource amount</param>
    /// <param name="timeQuantum">Random time to increase the specified resource</param>
    private System.Collections.IEnumerator IIncreaseResourceQuantityByNameAndAmount(string name, int amount, int timeQuantum)
    {
        while (true)
        {
            yield return new WaitForSeconds(rand.Next(0, timeQuantum));
            IncreaseQuantity(name, rand.Next(0, amount+1));
        }
    }

    /// <summary>
    /// IEnumerator for randomizing decreasing the resource specified by parameter name, 
    /// at a rate specified by parameter timeQuantum and amount specified by parameter amount
    /// </summary>
    /// <param name="name">Name of the resource to decrease amount</param>
    /// <param name="amount">Random maximum quantity to decrease the resource amount</param>
    /// <param name="timeQuantum">Random time to decrease the specified resource</param>
    /// <returns></returns>
    private System.Collections.IEnumerator IDecreaseResourceQuantityByNameAndAmount(string name, int amount, int timeQuantum)
    {
        while (true)
        {
            yield return new WaitForSeconds(rand.Next(0, timeQuantum));
            DecreaseQuantity(name, rand.Next(0, amount + 1));
        }
    }

    /// <summary>
    /// Starts Random Resource Fluctuation for this Market
    /// </summary>
    public void StartResourceFluctuation()
    {
        if(randomizeResourceFluctuation==null)
            randomizeResourceFluctuation = IRandomizeResourceFluctuation();
        StartCoroutine(randomizeResourceFluctuation);
    }

    /// <summary>
    /// Stops Random Resource Fluctuation for this Market
    /// </summary>
    public void StopResourceFluctuation()
    {
        if (randomizeResourceFluctuation!=null)
            StopCoroutine(randomizeResourceFluctuation);
    }
}