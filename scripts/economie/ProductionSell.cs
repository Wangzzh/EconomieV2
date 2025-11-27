using System;
using System.Collections.Generic;

public partial class ProductionSell(Goods goods): Sell(goods)
{
    public double AvailableAmount { get; set; } = 0f;
    public double TargetSellAmount { get; set; } = 0f;

    public override double GetSellAmountAtPrice(double price) {
        if (AvailableAmount <= 0f || price <= 0f) {
            return 0f;
        }
        double toSell = AvailableAmount - base.LastPrice * (AvailableAmount - TargetSellAmount) / price;
        return Math.Max(toSell, 0f);
    }

}   