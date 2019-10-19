public class Inventory  
{
	public int quantity {
		private set;
		get;
	}

	private int totalSpent = 0;

	public int calculateAveragePrice(){
		return  quantity > 0 ? totalSpent / quantity : 0;
	}

	public void Buy(int qty, int totalBuyPrice){
		quantity += qty;
		totalSpent += totalBuyPrice;
	}

	public void Sell(int qty, int totalSellPrice){
		for(var i = 0; i<qty; i++){
			totalSpent -= calculateAveragePrice ();
			quantity -= 1;
		}
	}
}
