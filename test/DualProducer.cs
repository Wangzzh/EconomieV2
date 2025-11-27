using Godot;
using System;

public partial class DualProducer : Node
{
	
	public Goods food = new Goods("food");

	public Location location;

	public Production foodProducerA;
	public ProductionSell foodProducerSellA;
	public Production foodProducerB;
	public ProductionSell foodProducerSellB;

	public Production foodConsumer;
	public ProductionPurchase foodConsumerPurchase;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initializing location");

		foodProducerA = new Production();
		foodProducerSellA = new ProductionSell(food);
		foodProducerSellA.AvailableAmount = 2.0;
		foodProducerSellA.TargetSellAmount = 1.0;
		foodProducerA.Sells.Add(food, foodProducerSellA);

		foodProducerB = new Production();
		foodProducerSellB = new ProductionSell(food);
		foodProducerSellB.AvailableAmount = 0.0;
		foodProducerSellB.TargetSellAmount = 0.0;
		foodProducerB.Sells.Add(food, foodProducerSellB);

		foodConsumer = new Production();
		foodConsumerPurchase = new ProductionPurchase(food);
		foodConsumerPurchase.Budget = 5.0;
		foodConsumer.Purchases.Add(food, foodConsumerPurchase);

		location = new Location();
		location.GoodsList.Add(food);
		location.Productions.Add(foodProducerA);
		location.Productions.Add(foodProducerB);
		location.Productions.Add(foodConsumer);	

		GD.Print("Finished initializing location");
	}

	public void RunMarket()
	{
		GD.Print("Running market");
		location.CollectPurchasesAndSells();
		location.RunMarket();
		
		foodProducerSellA.AvailableAmount = foodProducerSellA.AvailableAmount - foodProducerSellA.SoldAmount + 1.0;
		foodProducerSellA.TargetSellAmount = Math.Max(foodProducerSellA.AvailableAmount - 1.0, 0.0);
		foodProducerSellB.AvailableAmount = foodProducerSellB.AvailableAmount - foodProducerSellB.SoldAmount + 1.0;
		foodProducerSellB.TargetSellAmount = Math.Max(foodProducerSellB.AvailableAmount - 1.0, 0.0);
		
		// foodConsumerPurchase.Budget = foodConsumerPurchase.Budget * (1.0 + (1.0 - foodConsumerPurchase.PurchasedAmount) / 2.0);

		GD.Print("=========== Producer A ===========");
		GD.Print("Sold amount: " + foodProducerSellA.SoldAmount.ToString("F2"));
		GD.Print("Sold value: " + foodProducerSellA.SoldValue.ToString("F2"));
		GD.Print("Available: " + foodProducerSellA.AvailableAmount.ToString("F2"));
		GD.Print("Next target sell: " + foodProducerSellA.TargetSellAmount.ToString("F2"));

		GD.Print("=========== Producer B ===========");
		GD.Print("Sold amount: " + foodProducerSellB.SoldAmount.ToString("F2"));
		GD.Print("Sold value: " + foodProducerSellB.SoldValue.ToString("F2"));
		GD.Print("Available: " + foodProducerSellB.AvailableAmount.ToString("F2"));
		GD.Print("Next target sell: " + foodProducerSellB.TargetSellAmount.ToString("F2"));

		GD.Print("=========== Consumer ===========");
		GD.Print("Purchased amount: " + foodConsumerPurchase.PurchasedAmount.ToString("F2"));
		GD.Print("Purchased value: " + foodConsumerPurchase.PurchasedValue.ToString("F2"));
		GD.Print("Next budget: " + foodConsumerPurchase.Budget.ToString("F2"));
		
		GD.Print("================================");
	}
}
