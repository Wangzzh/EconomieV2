using Godot;
using System;

public partial class EcSellOrder : EcGameObject
{
    [Export]
    public int itemId;

    [Export]
    public int ownerOutputSectorId;

    [Export]
    public string purchaseOrderName = "Unnamed purchase";
    
    // This should be managed by market
    [Export]
    public double LastAmount { get; set; } = 0f;
    
    // This should be managed by market
    [Export]
    public double LastValue { get; set; } = 0f;
    
    // This should be managed by market
    [Export]
	public double LastPrice { get; set; } = 1.0f;

    // This should be managed by output sector
    [Export]
    public double DesiredAmount = 0.0;

    // This should be managed by output sector
    [Export]
    public double MaxAmount = 0.0;

    // This should be managed by output sector
    [Export]
    public bool Active = false;
    
    // This is invoked by the market to estimate price
	public double GetSellAmountAtPrice(double price)
    {
        if (!Active || MaxAmount <= 0f || price <= 0f) {
            return 0.0f;
        }
        // Evaluate minimum intrinsic value = 0.1
        return Math.Max(MaxAmount - Math.Max(LastPrice, 0.1) * (MaxAmount - DesiredAmount) / price, 0.0f);
    }

    public void ExecuteSell(double price, double amount)
    {
        LastPrice = price;
        LastAmount = amount;
        LastValue = amount * price;
    }
}