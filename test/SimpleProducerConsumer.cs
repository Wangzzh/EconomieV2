using Godot;
using System;

public partial class SimpleProducerConsumer : Node
{
	
	public Goods food = new Goods("food");

	public Location location;

	public Production foodProducer;
	public ProductionSell foodProducerSell;

	public Production foodConsumer;
	public ProductionPurchase foodConsumerPurchase;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initializing location");

		foodProducer = new Production();
		foodProducerSell = new ProductionSell(food);
		foodProducerSell.AvailableAmount = 2.0;
		foodProducerSell.TargetSellAmount = 1.0;
		foodProducer.Sells.Add(food, foodProducerSell);

		foodConsumer = new Production();
		foodConsumerPurchase = new ProductionPurchase(food);
		foodConsumerPurchase.Budget = 5.0;
		foodConsumer.Purchases.Add(food, foodConsumerPurchase);

		location = new Location();
		location.GoodsList.Add(food);
		location.Productions.Add(foodProducer);
		location.Productions.Add(foodConsumer);	

		GD.Print("Finished initializing location");
	}

	public void RunMarket()
	{
		GD.Print("Running market");
		location.CollectPurchasesAndSells();
		location.RunMarket();
		
		foodProducerSell.AvailableAmount = foodProducerSell.AvailableAmount - foodProducerSell.SoldAmount + 1.0;
		foodProducerSell.TargetSellAmount = foodProducerSell.AvailableAmount - 1.0;
		
		// foodConsumerPurchase.Budget = foodConsumerPurchase.Budget * (1.0 + (1.0 - foodConsumerPurchase.PurchasedAmount) / 2.0);

		GD.Print("=========== Producer ===========");
		GD.Print("Sold amount: " + foodProducerSell.SoldAmount.ToString("F2"));
		GD.Print("Sold value: " + foodProducerSell.SoldValue.ToString("F2"));
		GD.Print("Available: " + foodProducerSell.AvailableAmount.ToString("F2"));
		GD.Print("Next target sell: " + foodProducerSell.TargetSellAmount.ToString("F2"));

		GD.Print("=========== Consumer ===========");
		GD.Print("Purchased amount: " + foodConsumerPurchase.PurchasedAmount.ToString("F2"));
		GD.Print("Purchased value: " + foodConsumerPurchase.PurchasedValue.ToString("F2"));
		GD.Print("Next budget: " + foodConsumerPurchase.Budget.ToString("F2"));
		
		GD.Print("================================");
	}
}
