using Godot;
using System;

public partial class EcStorage : EcGameObject
{
    [Export]
    public int ItemId;

    [Export]
    public double Capacity = 5.0;

    [Export]
    public double Amount = 0.0;

    [Export]
    public double DesiredUnitAmount = 1.0;

    public void ScaleByUnitAmount(double newDesiredUnitAmount)
    {
        Capacity *= (newDesiredUnitAmount / DesiredUnitAmount);
        DesiredUnitAmount = newDesiredUnitAmount;
    }

    public double GetMaxInputAmount()
    {
        return Math.Max(Capacity - Amount, 0.00);
    }

    public double GetMaxOutputAmount()
    {
        return Math.Max(Amount, 0.0);
    }
}