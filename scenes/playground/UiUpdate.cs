using Godot;
using System;
using System.Linq;

public partial class UiUpdate : Node
{
	[Export]
	public Setup SetupNode;
	[Export]
	public Container Container;
	[Export]
	public Container ProductionContainer;
	[Export]
	public Container GoodsContainer;

	public override void _Process(double delta)
	{
		Location location = SetupNode.location;
		while (Container.GetChildCount() < location.Productions.Count)
		{
			Node productionNode = ProductionContainer.Duplicate();
			Container.AddChild(productionNode);
		}
		while(Container.GetChildCount() > location.Productions.Count)
		{
			Container.RemoveChild(Container.GetChild(0));
		}
		for (int i = 0; i < location.Productions.Count; i++)
		{
			Production production = location.Productions[i];
			Node productionContainer = Container.GetChild(i);
			Label productionName = (Label) productionContainer.FindChild("ProductionName", true, false);
			productionName.Text = production.Name;
			Label efficiency = (Label) productionContainer.FindChild("Efficiency", true, false);
			efficiency.Text = (production.LastEfficiency * 100.0).ToString("F0") + "%";
			Label profit = (Label) productionContainer.FindChild("Profit", true, false);
			profit.Text = "$" + production.LastProfit.ToString("F2");

			Container inputsContainer = (Container) productionContainer.FindChild("InputGoods", true, false);
			while (inputsContainer.GetChildCount() < production.Inputs.Count)
			{
				Node goodsNode = GoodsContainer.Duplicate();
				inputsContainer.AddChild(goodsNode);
			}
			while(inputsContainer.GetChildCount() > production.Inputs.Count)
			{
				inputsContainer.RemoveChild(inputsContainer.GetChild(0));
			}

			for (int j = 0; j < production.Inputs.Count; j++)
			{
				Goods key = production.Inputs.Keys.ElementAt(j);
				Node goodsContainer = inputsContainer.GetChild(j);
				Label multi = (Label) goodsContainer.FindChild("Multi", true, false);
				multi.Text = production.Inputs[key].ToString() + "x";
				Label goods = (Label) goodsContainer.FindChild("Goods", true, false);
				goods.Text = key.Name;
				ProgressBar cashBar = (ProgressBar) goodsContainer.FindChild("CashBar", true, false);
				cashBar.Value = production.InputCashPools[key].Amount / production.InputCashPools[key].Capacity;
				Label cashLabel = (Label) goodsContainer.FindChild("CashLabel", true, false);
				cashLabel.Text = "$" + production.InputCashPools[key].Amount.ToString("F2") + "/" + production.InputCashPools[key].Capacity.ToString("F2");
				ProgressBar goodsBar = (ProgressBar) goodsContainer.FindChild("GoodsBar", true, false);
				goodsBar.Value = production.InputPools[key].Amount / production.InputPools[key].Capacity;
				Label goodsLabel = (Label) goodsContainer.FindChild("GoodsLabel", true, false);
				goodsLabel.Text = production.InputPools[key].Amount.ToString("F2") + "/" + production.InputPools[key].Capacity.ToString("F2");
				Label amount = (Label) goodsContainer.FindChild("Amount", true, false);
				amount.Text = production.Purchases[key].PurchasedAmount.ToString("F2") + "@";
				Label price = (Label) goodsContainer.FindChild("Price", true, false);
				price.Text = "$" + production.Purchases[key].LastPrice.ToString("F2");
			}

			Container outputsContainer = (Container) productionContainer.FindChild("OutputGoods", true, false);
			while (outputsContainer.GetChildCount() < production.Outputs.Count)
			{
				Node goodsNode = GoodsContainer.Duplicate();
				outputsContainer.AddChild(goodsNode);
			}
			while(outputsContainer.GetChildCount() > production.Outputs.Count)
			{
				outputsContainer.RemoveChild(outputsContainer.GetChild(0));
			}

			for (int j = 0; j < production.Outputs.Count; j++)
			{
				Goods key = production.Outputs.Keys.ElementAt(j);
				Node goodsContainer = outputsContainer.GetChild(j);
				Label multi = (Label) goodsContainer.FindChild("Multi", true, false);
				multi.Text = production.Outputs[key].ToString() + "x";
				Label goods = (Label) goodsContainer.FindChild("Goods", true, false);
				goods.Text = key.Name;
				// Reverse cash and goods
				ProgressBar cashBar = (ProgressBar) goodsContainer.FindChild("GoodsBar", true, false);
				cashBar.Value = production.OutputCashPools[key].Amount / production.OutputCashPools[key].Capacity;
				Label cashLabel = (Label) goodsContainer.FindChild("GoodsLabel", true, false);
				cashLabel.Text = "$" + production.OutputCashPools[key].Amount.ToString("F2") + "/" + production.OutputCashPools[key].Capacity.ToString("F2");
				ProgressBar goodsBar = (ProgressBar) goodsContainer.FindChild("CashBar", true, false);
				goodsBar.Value = production.OutputPools[key].Amount / production.OutputPools[key].Capacity;
				Label goodsLabel = (Label) goodsContainer.FindChild("CashLabel", true, false);
				goodsLabel.Text = production.OutputPools[key].Amount.ToString("F2") + "/" + production.OutputPools[key].Capacity.ToString("F2");
				Label amount = (Label) goodsContainer.FindChild("Amount", true, false);
				amount.Text = production.Sells[key].SoldAmount.ToString("F2") + "@";
				Label price = (Label) goodsContainer.FindChild("Price", true, false);
				price.Text = "$" + production.Sells[key].LastPrice.ToString("F2");
			}
		}
	}
}
