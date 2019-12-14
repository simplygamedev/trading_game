using System;

public abstract class Resources : Subject<int>
{
	public abstract Curve demand { get; protected set; }

    public float equilibrium_price { protected set; get; }
    public float equilibrium_qty { protected set; get; }

    public float price_elasticity { protected set; get; }
    public float demand_shift { protected set; get; }
    public float price_shift { protected set; get; }

    public Resources(float initialAmount, float equilibrium_price, float equilibrium_qty, float price_elasticity, float demand_shift, float price_shift)
	{
		state = (int)initialAmount;
        this.equilibrium_price = equilibrium_price;
        this.equilibrium_qty = equilibrium_qty;
        this.price_elasticity = price_elasticity;
        this.demand_shift = demand_shift;
        this.price_shift = price_shift;
	}

    public bool DecreaseQuantity(int amount)
    {
        if (state - amount >= 0)
        {
            setState(state - amount);
            return true;
        }
        return false;
    }

    public bool IncreaseQuantity(int amount)
    {
        setState(state + amount);
        return true;
    }

    public abstract float calculatePrice();
    public abstract float calculateTotalPrice(int quantity);


    /// <summary>
    /// private method that allows the Market class to create new resources
    /// </summary>
    /// <param name="resourceName">Class name of the resource instance/object to create</param>
    /// <param name="initialValues[0]">Initial amount of this resource to create for this market when the game begins</param>
    /// <param name="initialValues[1]">Equilibrium quantity of the resource, we may wish to shift this parameter to be handled by the subclass resource though</param>
    /// <param name="initialValues[3]">Price Elasticity of the Resource</param>
    /// <param name="initialValues[4]">Demand Shift to use, this is normally set to 0</param>
    /// <param name="initialValues[5]">Price Shift to use, this is normally set to 0</param>

    /// <returns>The subclass resource object</returns>
    public static Resources createNewResourceByName(string resourceName, params float[] initialValues)
    {
        if (initialValues.Length < 6)
            throw new Exception("Unable to create new Resource object, Argument passed to initialValues parameter is not correct");

        return (Resources)Activator.CreateInstance(Type.GetType(resourceName), initialValues);
    }

}