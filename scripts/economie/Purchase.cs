using System;
using System.Collections.Generic;

public abstract partial class Purchase(Goods goods) {
	
    public Goods Goods { get; set; } = goods;
    public double PurchasedAmount { get; set; } = 0f;
    public double PurchasedValue { get; set; } = 0f;
	public double LastPrice { get; set; } = 0f;

	public abstract double GetPurchaseAmountAtPrice(double price);
}
