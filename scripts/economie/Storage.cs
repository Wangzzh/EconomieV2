using Godot;
using System;
using System.Collections.Generic;

public partial class Storage(Goods goods, double level = 1.0)
{
    public double Level = level; // The unit amount of each input or output
    public double Capacity = 5.0f * level;

    public double Amount = 0.0f;
    public Goods Goods = goods;

    // Requirements
    // Soft lower bound > Output amount cap > Output amount desired
    // 1 - Soft upper bound > Input amount cap > Input amount desired
    public double SoftUpperBound = 0.6f; // Max input is reduced if more than 60% full
    public double SoftLowerBound = 0.4f; // Max output is reduced if less than 40% full
    public double InputAmountDesired = 0.2f; // Desired input is capped at 20% of capacity
    public double OutputAmountDesired = 0.2f; // Desired output is capped at 20% of capacity
    public double InputAmountCap = 0.3f; // Max input is capped at 30% of capacity
    public double OutputAmountCap = 0.3f; // Max output is capped at 30% of capacity

    public void ModifyCapacity(double newLevel)
    {
        Capacity = newLevel * 10.0f;
        Level = newLevel;
    }

    public double GetMaxOutputAmount()
    {
        if (Amount / Capacity >= SoftLowerBound)
        {
            return OutputAmountCap * Capacity;
        }
        return OutputAmountCap * Amount / SoftLowerBound;
    }

    public double GetDesiredOutputAmount()
    {
        if (Amount / Capacity >= SoftLowerBound)
        {
            return OutputAmountDesired * Capacity;
        }
        return OutputAmountDesired * Amount / SoftLowerBound;
        
    }

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

    public double GetDesiredInputAmount()
    {
        if (Amount / Capacity <= SoftUpperBound)
        {
            return InputAmountDesired * Capacity;
        }
        else if (Amount >= Capacity)
        {
            return 0.0f;
        }
        return InputAmountDesired * (Capacity - Amount) / (1.0 - SoftUpperBound);
    }

    public void RunDecay()
    {
        if (Goods.Decay > 0.0 && Amount > 0.0)
        {
            Amount *= (1.0 - Goods.Decay);
        }
    }

    public override string ToString()
    {
        return "Storing " + Goods.Name + " of amount " + Amount.ToString("F2") + "/" + Capacity.ToString("F2") + "\n";
    }
}