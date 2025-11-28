using System;
using System.Collections.Generic;

public partial class Sell(Storage goodsPool, Storage cashPool) {
    

    public Storage GoodsPool = goodsPool;
    public Storage CashPool = cashPool;

    public double SoldAmount { get; set; } = 0f;
    public double SoldValue { get; set; } = 0f;
	public double LastPrice { get; set; } = 0f;

    // This is invoked by the market to estimate price
	public double GetSellAmountAtPrice(double price)
    {
        double desired = GetMaxSellAmountByGoodsPool();
        double available = desired * 2.0;
        if (available <= 0f || price <= 0f) {
            return 0.0f;
        }
        return Math.Max(available - Math.Max(LastPrice, 0.1) * (available - desired) / price, 0.0f);
    }

    public double GetMaxSellAmountByGoodsPool()
    {
        return GoodsPool.GetMaxOutputAmount();
    }

    public void ExecuteSell(double price)
    {
        if (price > 0.0) 
        {
            SoldAmount = GetSellAmountAtPrice(price);
            LastPrice = price;
            SoldValue = SoldAmount * price;
        } 
        else
        {
            SoldAmount = 0.0;
            LastPrice = 0.0;
            SoldValue = 0.0;
        }
        CashPool.Amount += SoldValue;
        GoodsPool.Amount -= SoldAmount;
    }
    
    public override string ToString()
    {
        return "Sold " + SoldAmount.ToString("F2") + " " + goodsPool.Goods.Name + "@" + LastPrice.ToString("F2") + " = " + SoldValue.ToString("F2") + "\n";
    }
}