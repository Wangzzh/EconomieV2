using Godot;
using System;
using System.Collections.Generic;

public partial class Storage(Goods goods, double level = 1.0)
{
    public double Level = level; // The unit amount of each input or output
    public double Capacity = 10.0f * level;

    public double Amount = 0.0f;
    public Goods Goods = goods;

    public double SoftUpperBound = 0.6f; // Max input is reduced if more than 60% full
    public double SoftLowerBound = 0.4f; // Max output is reduced if less than 40% full
    public double InputAmountCap = 0.1f; // Max input is capped at 10% of capacity
    public double OutputAmountCap = 0.1f; // Max output is capped at 20% of capacity

    public void ModifyCapacity(double newLevel)
    {
        Capacity = newLevel * 10.0f;
        Level = newLevel;
    }

    // Limit possible output from the storage
    public double GetMaxOutputAmount()
    {
        if (Amount / Capacity >= SoftLowerBound)
        {
            return OutputAmountCap * Capacity;
        }
        return OutputAmountCap * Amount / SoftLowerBound;
    }

    // Limit desired input from storage, but it is possible to receive higher inputs that exceeds capacity
    public double GetMaxInputAmount()
    {
        if (Amount / Capacity <= SoftUpperBound)
        {
            return InputAmountCap * Capacity;
        }
        else if (Amount >= Capacity)
        {
            return 0.0f;
        }
        return InputAmountCap * (Capacity - Amount) / (1.0 - SoftUpperBound);
    }

    public override string ToString()
    {
        return "Storing " + Goods.Name + " of amount " + Amount.ToString("F2") + "/" + Capacity.ToString("F2") + "\n";
    }
}