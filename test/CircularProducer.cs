using Godot;
using System;

public partial class CircularProducer : Node
{
	
	public Goods food = new Goods("food");
	public Goods can = new Goods("can");

	public Location location;

	public Production foodProducer;
	public ProductionSell foodProducerSell;
	public ProductionPurchase foodProducerPurchase;

	public Production canProducer;
	public ProductionSell canProducerSell;
	public ProductionPurchase canProducerPurchase;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initializing location");

		foodProducer = new Production();
		foodProducerPurchase = new ProductionPurchase(can);
		foodProducerPurchase.Budget = 5.0;
		foodProducerPurchase.MaxPurchaseAmount = 1.1;
		foodProducer.Purchases.Add(can, foodProducerPurchase);
		foodProducerSell = new ProductionSell(food);
		foodProducerSell.AvailableAmount = 2.0;
		foodProducerSell.TargetSellAmount = 1.0;
		foodProducer.Sells.Add(food, foodProducerSell);
		foodProducer.Cash = 10.0;

		canProducer = new Production();
		canProducerPurchase = new ProductionPurchase(food);
		canProducerPurchase.Budget = 5.0;
		canProducerPurchase.MaxPurchaseAmount = 1.1;
		canProducer.Purchases.Add(food, canProducerPurchase);
		canProducerSell = new ProductionSell(food);
		canProducerSell.AvailableAmount = 1.0;
		canProducerSell.TargetSellAmount = 0.0;
		canProducer.Sells.Add(can, canProducerSell);
		canProducer.Cash = 10.0;

		location = new Location();
		location.GoodsList.Add(food);
		location.GoodsList.Add(can);
		location.Productions.Add(foodProducer);
		location.Productions.Add(canProducer);	

		GD.Print("Finished initializing location");
	}

	public void RunMarket()
	{
		GD.Print("Running market");
		location.CollectPurchasesAndSells();
		location.RunMarket();
		
		foodProducerSell.AvailableAmount = foodProducerSell.AvailableAmount - foodProducerSell.SoldAmount + foodProducerPurchase.PurchasedAmount * 0.5;
		foodProducerSell.TargetSellAmount = foodProducerSell.AvailableAmount - 1.0;
		
		canProducerSell.AvailableAmount = canProducerSell.AvailableAmount - canProducerSell.SoldAmount + canProducerPurchase.PurchasedAmount * 2.0;
		canProducerSell.TargetSellAmount = canProducerSell.AvailableAmount - 1.0;

		GD.Print("=========== Food Producer ===========");
		GD.Print("Sold amount: " + foodProducerSell.SoldAmount.ToString("F2"));
		GD.Print("Sold value: " + foodProducerSell.SoldValue.ToString("F2"));
		GD.Print("Available: " + foodProducerSell.AvailableAmount.ToString("F2"));
		GD.Print("Next target sell: " + foodProducerSell.TargetSellAmount.ToString("F2"));
		GD.Print("Purchased amount: " + foodProducerPurchase.PurchasedAmount.ToString("F2"));
		GD.Print("Purchased value: " + foodProducerPurchase.PurchasedValue.ToString("F2"));
		GD.Print("Next budget: " + foodProducerPurchase.Budget.ToString("F2"));

		GD.Print("=========== Can Producer ===========");
		GD.Print("Sold amount: " + canProducerSell.SoldAmount.ToString("F2"));
		GD.Print("Sold value: " + canProducerSell.SoldValue.ToString("F2"));
		GD.Print("Available: " + canProducerSell.AvailableAmount.ToString("F2"));
		GD.Print("Next target sell: " + canProducerSell.TargetSellAmount.ToString("F2"));
		GD.Print("Purchased amount: " + canProducerPurchase.PurchasedAmount.ToString("F2"));
		GD.Print("Purchased value: " + canProducerPurchase.PurchasedValue.ToString("F2"));
		GD.Print("Next budget: " + canProducerPurchase.Budget.ToString("F2"));

		GD.Print("================================");
	}
}
