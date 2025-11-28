using Godot;
using System;
using System.Collections.Generic;

public partial class SimpleProducerConsumer : Node
{
	
	public Goods food = new("food");
	public Goods wheat = new("wheat");
	public Goods currency = new("Currency");

	public Location location;

	public Production wheatProvider;
	public Production foodProduction;
	public Production foodConsumer;

	[Export]
	public Label PriceLabel;
	
	[Export]
	public Label ProfitLabel;

	[Export]
	public Label EfficiencyLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initializing location");

		Dictionary<Goods, double> wheatProviderOutputs = new(){{wheat, 1.0}};
		wheatProvider = new([], wheatProviderOutputs, currency);
		
		Dictionary<Goods, double> foodProductionInputs = new(){{wheat, 1.0}};
		Dictionary<Goods, double> foodProductionOutputs = new(){{food, 1.0}};
		foodProduction = new(foodProductionInputs, foodProductionOutputs, currency);
		foodProduction.InputCashPools[wheat].Amount = 5.0;
		
		Dictionary<Goods, double> foodConsumerInputs = new(){{food, 1.0}};
		foodConsumer = new(foodConsumerInputs, [], currency);

		location = new()
		{
			Productions = [wheatProvider, foodProduction, foodConsumer],
			GoodsList = [food, wheat, currency]
		};
		GD.Print("Finished initializing location");
	}

	public void RunMarket()
	{
		foodConsumer.InputCashPools[food].Amount = 6.0 * foodConsumer.Inputs[food];
		wheatProvider.OutputCashPools[wheat].Amount = 1.0;

		GD.Print("##### Running market");
		location.CollectPurchasesAndSells();
		location.RunMarket();
		
		GD.Print("##### Running productions");
		location.RunProduction();

		GD.Print("##### Printing productions");
		GD.Print(wheatProvider.ToString());
		GD.Print(foodProduction.ToString());
		GD.Print(foodConsumer.ToString());
	}

	public void UpdateWheatOut(double value)
	{
		wheatProvider.Outputs = new(){{wheat, value}};
		wheatProvider.OutputCashPools[wheat].ModifyCapacity(value);
		wheatProvider.OutputPools[wheat].ModifyCapacity(value);
	}

	public void UpdateWheatIn(double value)
	{
		foodProduction.Inputs = new(){{wheat, value}};
		foodProduction.InputCashPools[wheat].ModifyCapacity(value);
		foodProduction.InputPools[wheat].ModifyCapacity(value);
	}

	public void UpdateFoodOut(double value)
	{
		foodProduction.Outputs = new(){{food, value}};
		foodProduction.OutputCashPools[food].ModifyCapacity(value);
		foodProduction.OutputPools[food].ModifyCapacity(value);
	}

	public void UpdateFoodIn(double value)
	{
		foodConsumer.Inputs = new(){{food, value}};
		foodConsumer.InputCashPools[food].ModifyCapacity(value);
		foodConsumer.InputPools[food].ModifyCapacity(value);
	}

	public override void _Process(double delta)
	{
		PriceLabel.Text = "Price: Wheat@" + foodProduction.Purchases[wheat].LastPrice.ToString("F2") 
			+ " Food@" + foodProduction.Sells[food].LastPrice.ToString("F2");
		ProfitLabel.Text = "Profit: Wheat@" + wheatProvider.LastProfit.ToString("F2") 
			+ " Food@" + foodProduction.LastProfit.ToString("F2");
		EfficiencyLabel.Text = "Eff: Wheat@" + wheatProvider.LastEfficiency.ToString("F2") 
			+ " Food@" + foodProduction.LastEfficiency.ToString("F2");
	}
}
