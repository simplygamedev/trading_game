public abstract class Resources : Subject<int> 
{
	protected int equilibriumQuantity;
	protected Curve demand;

	public Resources(int initialAmount, int equilibriumQuantity, Curve demand)
	{
		state = initialAmount;
		this.equilibriumQuantity = equilibriumQuantity;
		this.demand = demand;
	}

	public void minusResources(int amount)
	{
		if (state - amount >= 0) {
			setState(state - amount);
		}
	}

	public void increaseResources(int amount)
	{
		setState(state + amount);
	}

	public virtual int calculatePrice(){
		int price = calculateTotalPrice(1);
		return (price > 0) ? price : 1 ;
	}

	public virtual int calculateTotalPrice(int quantity){

		int quantityLeftAfterPurchase = state - quantity;

		return (int)demand.calculateAreaUnderCurve(state, quantityLeftAfterPurchase);
	}
}