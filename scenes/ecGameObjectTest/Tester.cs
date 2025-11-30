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

		EcProductionMethod bakery = new()
		{
			ProductionMethodName = "Bakery",
			InputItemAmounts = {{wheat.Id, 2.0}},
			OutputItemAmounts = {{food.Id, 1.0}}
		};
		bakery.StoreAsGameObject();
	}

}
