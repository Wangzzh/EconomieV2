using Godot;
using System;

public partial class EcPurchaseOrder : EcGameObject
{
    [Export]
    public int itemId;

    [Export]
    public int ownerInputSectorId;

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
    public double Budget = 0.0;

    // This should be managed by output sector
    [Export]
    public double MaxAmount = 0.0;

    // This should be managed by output sector
    [Export]
    public bool Active = false;
    
    // This is invoked by the market to estimate price
	public double GetPurchaseAmountAtPrice(double price)
    {
        if (!Active || Budget <= 0.0f || price <= 0.0f)
        {
            return 0.0f;
        }
        return Math.Min(Budget / price, MaxAmount);
    }

    public void ExecutePurchase(double price, double amount)
    {
        LastPrice = price;
        LastAmount = amount;
        LastValue = amount * price;
    }
}