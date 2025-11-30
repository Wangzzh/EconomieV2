using Godot;
using System;

public partial class EcPurchaseOrder : EcGameObject
{
    [Export]
    public int ownerInputSectorId;

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
    public bool Active = true;
    
    // This is invoked by the market to estimate price
	public double GetPurchaseAmountAtPrice(double price)
    {
        double budget = DesiredAmount * LastPrice;
        if (!Active || budget <= 0.0f || price <= 0.0f)
        {
            return 0.0f;
        }
        return Math.Min(budget / price, MaxAmount);
    }

    public void ExecutePurchase(double price, double amount)
    {
        LastPrice = price;
        LastAmount = amount;
        LastValue = amount * price;
        if (Active) 
        {
            EcProductionInputSector inputSector = GetGameObject<EcProductionInputSector>(ownerInputSectorId);
            inputSector.ExecutePurchase();
        }
    }
}