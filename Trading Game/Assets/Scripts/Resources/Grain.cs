/// <summary>
/// 
/// </summary>
/// <remarks>
/// Price Elasticity must be negative
/// </remarks>
public class Grain : Resources
{
    public override Curve demand
    {
        get;
        protected set;
    }

    public Grain(float initialAmount, float equilibrium_price, float equilibrium_qty, float price_elasticity, float demand_shift, float price_shift) : base(initialAmount, equilibrium_price, equilibrium_qty, price_elasticity, demand_shift, price_shift)
    {
        demand = Curve.createCurveByName("StraightCurve", price_elasticity, demand_shift, equilibrium_price, equilibrium_qty);
    }

    public Grain(params float[] initialValues) : this(initialValues[0], initialValues[1], initialValues[2], initialValues[3], initialValues[4], initialValues[5]) { }

    public override float calculatePrice()
    {
        float price = demand.calculateY(getState());
        return (price > 0) ? price : 1;
    }

    public override float calculateTotalPrice(int quantity)
    {
        int quantityLeftAfterPurchase = state - quantity;
        return demand.calculateAreaUnderCurve(state, quantityLeftAfterPurchase);
    }
}