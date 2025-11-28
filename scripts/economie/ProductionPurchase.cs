using System;
using System.Collections.Generic;

public partial class ProductionPurchase(Goods goods): Purchase(goods)
{
    public double Budget { get; set; } = 0f;
    public double MaxPurchaseAmount = Double.MaxValue;

    public override double GetPurchaseAmountAtPrice(double price) {
        if (Budget <= 0f || price <= 0f) {
            return 0f;
        }
        return Math.Min(Budget / price, MaxPurchaseAmount);
    }
}