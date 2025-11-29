using System;
using System.Collections.Generic;

public partial class Purchase(Storage cashPool, Storage goodsPool) {
	
    public Storage CashPool = cashPool;
    public Storage GoodsPool = goodsPool;

    public double PurchasedAmount { get; set; } = 0f;
    public double PurchasedValue { get; set; } = 0f;
	public double LastPrice { get; set; } = 1.0f;

    // This is invoked by the market to estimate price
	public double GetPurchaseAmountAtPrice(double price)
    {
        double budget = CashPool.GetDesiredOutputAmount();
        if (budget <= 0.0f || price <= 0.0f)
        {
            return 0.0f;
        }
        return Math.Min(budget / price, GoodsPool.GetMaxInputAmount());
    }

    public void ExecutePurchase(double price)
    {
        if (price > 0.0) 
        {
            PurchasedAmount = GetPurchaseAmountAtPrice(price);
            LastPrice = price;
            PurchasedValue = PurchasedAmount * price;
        } else
        {
            PurchasedAmount = 0.0;
            LastPrice = 0.0;
            PurchasedValue = 0.0;
        }
        CashPool.Amount -= PurchasedValue;
        GoodsPool.Amount += PurchasedAmount;
    }

    public override string ToString()
    {
        return "Purchased " + PurchasedAmount.ToString("F2") + " " + GoodsPool.Goods.Name + "@" + LastPrice.ToString("F2") + " = " + PurchasedValue.ToString("F2") + "\n";
    }

}
