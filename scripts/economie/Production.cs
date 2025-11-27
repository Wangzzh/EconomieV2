using System;
using System.Collections.Generic;

public partial class Production
{
    public double Cash { get; set; }
	
    public Dictionary<Goods, double> Inventory { get; set; } = new Dictionary<Goods, double>();
	public Dictionary<Goods, double> TargetInventory { get; set; } = new Dictionary<Goods, double>();

	public Dictionary<Goods, ProductionPurchase> Purchases { get; set; } = new Dictionary<Goods, ProductionPurchase>();
	public Dictionary<Goods, ProductionSell> Sells { get; set; } = new Dictionary<Goods, ProductionSell>();
}
