using System;
using System.Collections.Generic;
using Godot;

public partial class Production
{
	public Dictionary<Goods, double> Inputs = [];
	public Dictionary<Goods, double> Outputs = [];

	public Dictionary<Goods, Storage> InputCashPools = [];
	public Dictionary<Goods, Storage> OutputCashPools = [];
	public Dictionary<Goods, Storage> InputPools = [];
	public Dictionary<Goods, Storage> OutputPools = [];

	public Dictionary<Goods, Purchase> Purchases = [];
	public Dictionary<Goods, Sell> Sells = [];

	public double LastEfficiency = 0.0;
	public double LastProfit = 0.0;


	public Production(
		Dictionary<Goods, double> inputs, 
		Dictionary<Goods, double> outputs, 
		Goods currency)
	{
		Inputs = inputs;
		Outputs = outputs;

		InputCashPools = [];
		OutputCashPools = [];
		InputPools = [];
		OutputPools = [];
		Purchases = [];
		Sells = [];
		foreach (KeyValuePair<Goods, double> input in inputs)
		{
			Storage inputCashPool = new(currency, input.Value * 1.0); // Default price to 1.0?
			InputCashPools.Add(input.Key, inputCashPool);
			Storage inputPool = new(input.Key, input.Value);
			InputPools.Add(input.Key, inputPool);
			Purchase purchase = new(inputCashPool, inputPool);
			Purchases.Add(input.Key, purchase);
		}
		foreach (KeyValuePair<Goods, double> output in outputs)
		{
			Storage outputCashPool = new(currency, output.Value * 1.0); // Default price to 1.0?
			OutputCashPools.Add(output.Key, outputCashPool);
			Storage outputPool = new(output.Key, output.Value);
			OutputPools.Add(output.Key, outputPool);
			Sell sell = new(outputPool, outputCashPool);
			Sells.Add(output.Key, sell);
		}
	}

	public void RunProduction()
	{
		// Assume purchases and sells are handled by the market already
		
		// Calculate max efficiency
		double efficiency = 1.0f;
		foreach (KeyValuePair<Goods, double> input in Inputs) {
			efficiency = Math.Min(efficiency, InputPools[input.Key].GetMaxOutputAmount() / input.Value);
		}
		foreach (KeyValuePair<Goods, double> output in Outputs)
		{
			efficiency = Math.Min(efficiency, OutputPools[output.Key].GetMaxInputAmount() / output.Value);
		}

		// Execute
		foreach (KeyValuePair<Goods, double> input in Inputs) {
			InputPools[input.Key].Amount -= (input.Value * efficiency);
		}
		foreach (KeyValuePair<Goods, double> output in Outputs)
		{
			OutputPools[output.Key].Amount += (output.Value * efficiency);
		}

		LastEfficiency = efficiency;
	}

	public void DistributeCash()
	{
		double maxReinvestmentToInput = 0.0f;
		double maxReinvestmentFromOutput = 0.0f;

		foreach (KeyValuePair<Goods, double> input in Inputs) {
			double newLevel = input.Value * Math.Max(Purchases[input.Key].LastPrice, 0.1);
			InputCashPools[input.Key].ModifyCapacity(newLevel);
			maxReinvestmentToInput += InputCashPools[input.Key].GetMaxInputAmount();
		}
		foreach (KeyValuePair<Goods, double> output in Outputs)
		{
			double newLevel = output.Value * Math.Max(Sells[output.Key].LastPrice, 0.1);
			OutputCashPools[output.Key].ModifyCapacity(newLevel);
			maxReinvestmentFromOutput += OutputCashPools[output.Key].GetMaxOutputAmount();
			OutputCashPools[output.Key].Amount -= OutputCashPools[output.Key].GetMaxOutputAmount();
		}
		
		double profit = 0.0;
		double reinvestMultiplier = 1.0;
		if (maxReinvestmentFromOutput >= maxReinvestmentToInput)
		{
			profit = maxReinvestmentFromOutput - maxReinvestmentToInput;
			reinvestMultiplier = 1.0;
		} else
		{
			profit = 0.0;
			reinvestMultiplier = maxReinvestmentFromOutput / maxReinvestmentToInput;
		}

		LastProfit = profit;
		foreach (KeyValuePair<Goods, double> input in Inputs) {
			InputCashPools[input.Key].Amount += InputCashPools[input.Key].GetMaxInputAmount() * reinvestMultiplier;
		}
	}

	public override string ToString()
	{
		string str = "============= Production ============\n";
		foreach (KeyValuePair<Goods, double> input in Inputs)
		{
			str += "[ " + input.Value + " " + input.Key.Name + " ] ";
		}
		str += "=> ";
		foreach (KeyValuePair<Goods, double> output in Outputs)
		{
			str += "[ " + output.Value + " " + output.Key.Name + " ] ";
		}
		str += "\n";
		str += "-------------------------------------\n";
		foreach (KeyValuePair<Goods, double> input in Inputs) {
			str += InputCashPools[input.Key].ToString();
			str += Purchases[input.Key].ToString();
			str += InputPools[input.Key].ToString();
		}
		str += "-------------------------------------\n";
		foreach (KeyValuePair<Goods, double> output in Outputs)
		{
			str += OutputPools[output.Key].ToString();
			str += Sells[output.Key].ToString();
			str += OutputCashPools[output.Key].ToString();
		}
		str += "-------------------------------------\n";
		str += "Efficiency = " + LastEfficiency + "\n";
		str += "Profit = " + LastProfit + "\n";
		str += "=====================================\n";
		return str;
	}
}
