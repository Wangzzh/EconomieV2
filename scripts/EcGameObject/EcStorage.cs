using Godot;
using System;

public partial class EcStorage : EcGameObject
{
    [Export]
    public string StorageName;
    
    [Export]
    public int ItemId;

    [Export]
    public double Capacity = MAX_CAPACITY_FACTOR;

    [Export]
    public double Amount = 0.0;

    [Export]
    public double DesiredUnitAmount = 1.0;

    static readonly double MAX_CAPACITY_FACTOR = 5.0;

    public void UpdateUnitAmount(double newDesiredUnitAmount)
    {
        Capacity = newDesiredUnitAmount * MAX_CAPACITY_FACTOR;
        DesiredUnitAmount = newDesiredUnitAmount;
    }

    public double GetMaxInputAmount()
    {
        return Math.Max(Capacity - Amount, 0.0);
    }

    public double GetMaxOutputAmount()
    {
        return Math.Max(Amount, 0.0);
    }
}