using Godot;
using System;
using System.Collections.Generic;

public partial class Setup : Node
{
	
	public Goods food = new("food", 0.01);
	public Goods wheat = new("wheat", 0.1);
	public Goods tools = new("tools", 0.001);
	public Goods wine = new("wine", 0.01);
	public Goods currency = new("Currency");
	public Location location;
	public Production wheatProvider;
	public Production toolsProvider;
	public Production foodFactory;
	public Production foodConsumer;
	public Production wineFactory;
	public Production wineConsumer;

	public override void _Ready()
	{
		GD.Print("Initializing location");

		wheatProvider = new(
			"Wheat Provider", 
			[], 
			new(){{wheat, 10.0}}, 
			currency
		);

		toolsProvider = new(
			"Tools Provider", 
			[], 
			new(){{tools, 4.0}}, 
			currency
		);

		foodFactory = new(
			"Food Factory", 
			new(){{wheat, 4.0}, {tools, 1.0}}, 
			new(){{food, 4.0}}, 
			currency
		);
		foodFactory.InputCashPools[wheat].Amount = 5.0;
		foodFactory.InputCashPools[tools].Amount = 5.0;
		
		wineFactory = new(
			"Wine Factory", 
			new(){{food, 4.0}, {tools, 0.5}}, 
			new(){{wine, 1.0}}, 
			currency
		);
		wineFactory.InputCashPools[food].Amount = 5.0;
		wineFactory.InputCashPools[tools].Amount = 5.0;

		foodConsumer = new(
			"Food Consumer", 
			new(){{food, 2.0}}, 
			[], 
			currency
		);

		wineConsumer = new(
			"Wine Consumer", 
			new(){{wine, 1.0}}, 
			[], 
			currency
		);

		location = new()
		{
			Productions = [wheatProvider, toolsProvider, foodFactory, wineFactory, foodConsumer, wineConsumer],
			GoodsList = [food, wheat, tools, wine, currency]
		};
		GD.Print("Finished initializing location");
	}

	public void RunMarket()
	{
		foodConsumer.InputCashPools[food].Amount = 5.0 * foodConsumer.Inputs[food];
		wineConsumer.InputCashPools[wine].Amount = 5.0 * wineConsumer.Inputs[wine];
		wheatProvider.OutputCashPools[wheat].Amount = 1.0;
		toolsProvider.OutputCashPools[tools].Amount = 1.0;

		GD.Print("##### Running market");
		location.CollectPurchasesAndSells();
		location.RunMarket();
		
		GD.Print("##### Running productions");
		location.RunProduction();

		// GD.Print("##### Printing productions");
		// GD.Print(wheatProvider.ToString());
		// GD.Print(foodProduction.ToString());
		// GD.Print(foodConsumer.ToString());
	}
}
