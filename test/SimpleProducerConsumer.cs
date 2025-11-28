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
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initializing location");

		Dictionary<Goods, double> wheatProviderOutputs = new(){{wheat, 2.0}};
		wheatProvider = new([], wheatProviderOutputs, currency);
		
		Dictionary<Goods, double> foodProductionInputs = new(){{wheat, 2.0}};
		Dictionary<Goods, double> foodProductionOutputs = new(){{food, 1.0}};
		foodProduction = new(foodProductionInputs, foodProductionOutputs, currency);
		foodProduction.InputCashPools[wheat].Amount = 5.0;
		
		Dictionary<Goods, double> foodConsumerInputs = new(){{food, 2.0}};
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
		foodConsumer.InputCashPools[food].Amount = 5.0;
		wheatProvider.OutputCashPools[wheat].Amount = 5.0;

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
}
