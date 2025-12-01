using Godot;
using System;

public partial class Tester : Node
{
	public override void _Ready()
	{
		EcItem wheat = new()
		{
			ItemName = "Wheat",
			Decay = 0.05
		};
		wheat.StoreAsGameObject();

		EcItem food = new()
		{
			ItemName = "Food"
		};
		food.StoreAsGameObject();

		EcItem gold = new()
		{
			ItemName = "Gold"
		};
		gold.StoreAsGameObject();

		EcProductionMethod baking = new()
		{
			ProductionMethodName = "Baking",
			InputItemAmounts = {{wheat.Id, 2.0}},
			OutputItemAmounts = {{food.Id, 1.0}}
		};
		baking.StoreAsGameObject();
		
		EcProductionMethod steaming = new()
		{
			ProductionMethodName = "Steaming",
			InputItemAmounts = {{wheat.Id, 5.0}},
			OutputItemAmounts = {{food.Id, 2.0}}
		};
		steaming.StoreAsGameObject();

		EcProductionSchema foodFactorySchema = new()
		{
			SchemaName = "Food Factory",
			ProductionMethodIds = [baking.Id, steaming.Id]
		};
		foodFactorySchema.StoreAsGameObject();

		EcProduction foodProduction = EcProduction.CreateProductionFromSchema(foodFactorySchema, gold);
	}

}
