using Godot;
using System;
using System.Collections.Generic;

public partial class Setup : Node
{
	
	public Goods food = new("food", 0.01);
	public Goods wheat = new("wheat", 0.1);
	public Goods tools = new("tools", 0.001);
	public Goods wine = new("wine", 0.01);
	public Goods labor = new("labor", 1.0, true);
	public Goods currency = new("Currency");
	public Location location;
	public Production wheatProvider;
	public Production toolsProvider;
	public Production laborProvider;
	public Production foodFactory;
	public Production foodConsumer;
	public Production wineFactory;
	public Production wineConsumer;

	public override void _Ready()
	{
		GD.Print("Initializing location");

		wheatProvider = new(
			"Wheat Provider", 
			new(){{labor, 1.0}}, 
			new(){{wheat, 8.0}}, 
			currency
		);

		toolsProvider = new(
			"Tools Provider", 
			new(){{labor, 1.0}}, 
			new(){{tools, 5.0}}, 
			currency
		);

		laborProvider = new(
			"Labor Provider", 
			[], 
			new(){{labor, 10.0}}, 
			currency
		);

		foodFactory = new(
			"Food Factory", 
			new(){{wheat, 4.0}, {tools, 1.0}, {labor, 5.0}}, 
			new(){{food, 1.0}}, 
			currency
		);
		foodFactory.InputCashPools[wheat].Amount = 5.0;
		foodFactory.InputCashPools[tools].Amount = 5.0;
		foodFactory.InputCashPools[labor].Amount = 5.0;
		
		wineFactory = new(
			"Wine Factory", 
			new(){{food, 4.0}, {tools, 0.5}, {labor, 5.0}}, 
			new(){{wine, 1.0}}, 
			currency
		);
		wineFactory.InputCashPools[food].Amount = 5.0;
		wineFactory.InputCashPools[tools].Amount = 5.0;
		wineFactory.InputCashPools[labor].Amount = 5.0;

		foodConsumer = new(
			"Food Consumer", 
			new(){{food, 5.0}}, 
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
			Productions = [wheatProvider, toolsProvider, laborProvider, foodFactory, wineFactory, foodConsumer, wineConsumer],
			GoodsList = [food, wheat, tools, wine, currency, labor]
		};
		GD.Print("Finished initializing location");
	}

	public void RunMarket()
	{
		foodConsumer.InputCashPools[food].Amount = 0.5 * foodConsumer.Inputs[food];
		wineConsumer.InputCashPools[wine].Amount = 5.0 * wineConsumer.Inputs[wine];
		wheatProvider.OutputCashPools[wheat].Amount = 1.0;
		toolsProvider.OutputCashPools[tools].Amount = 1.0;

		location.CollectPurchasesAndSells();
		location.RunMarket();
		location.CleanUpService();
		location.RunProduction();

		GD.Print(
			foodFactory.Purchases[wheat].LastPrice.ToString("F2") + "," +
			foodFactory.Purchases[tools].LastPrice.ToString("F2") + "," +
			foodFactory.Purchases[labor].LastPrice.ToString("F2") + "," +
			foodFactory.Sells[food].LastPrice.ToString("F2") + "," +
			wineFactory.Sells[wine].LastPrice.ToString("F2")
		);
	}
}
