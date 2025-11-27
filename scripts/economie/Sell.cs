using System;
using System.Collections.Generic;

public abstract partial class Sell(Goods goods) {
    
    public Goods Goods { get; set; } = goods;
    public double SoldAmount { get; set; } = 0f;
    public double SoldValue { get; set; } = 0f;
	public double LastPrice { get; set; } = 0f;

    public abstract double GetSellAmountAtPrice(double price);
}