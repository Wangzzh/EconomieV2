using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class Location {
    public List<Production> Productions = [];
    public List<Goods> GoodsList = [];

    public Dictionary<Goods, List<Purchase>> AllGoodsPurchases = [];
    public Dictionary<Goods, List<Sell>> AllGoodsSells = [];
    public Dictionary<Goods, double> GoodsPrices = [];

    public void CollectPurchasesAndSells() {
        AllGoodsPurchases.Clear();
        AllGoodsSells.Clear();
        foreach(Goods goods in GoodsList) {
            List<Purchase> purchaseList = [];
            List<Sell> sellList = [];
            foreach(Production production in Productions) {
                if (production.Purchases.ContainsKey(goods)) {
                    purchaseList.Add(production.Purchases[goods]);
                }
                if (production.Sells.ContainsKey(goods)) {
                    sellList.Add(production.Sells[goods]);
                }
            }
            AllGoodsPurchases.Add(goods, purchaseList);
            AllGoodsSells.Add(goods, sellList);
        }
    }

    public void RunMarket() {
        foreach(Goods goods in GoodsList) {
            GD.Print("Running market for goods: " + goods.Name);
            List<Purchase> purchaseList = AllGoodsPurchases[goods];
            List<Sell> sellList = AllGoodsSells[goods];
            if (purchaseList.Count == 0 || sellList.Count == 0) {
                GD.Print("Not enough sell or purchase for goods: " + goods.Name);
                foreach (Purchase purchase in purchaseList)
                {
                    purchase.ExecutePurchase(0.0);
                }
                foreach (Sell sell in sellList) 
                {
                    sell.ExecuteSell(0.0);
                }
                GoodsPrices[goods] = 0.0;
            }
            else {
                double low = 0.0;
                double current = 1.0;
                double high = Double.MaxValue;
                while (high - low >= 0.001) {
                    double purchaseAmount = GetPurchaseAmountAtPrice(current, purchaseList);
                    double sellAmount = GetSellAmountAtPrice(current, sellList);
                    if (purchaseAmount <= 0.01 && sellAmount <= 0.01)
                    {
                        current = 0.0;
                        break;
                    }
                    if (sellAmount - purchaseAmount >= 0.001) {
                        high = current;
                        current = (high + low) / 2.0;
                    }
                    else if (sellAmount - purchaseAmount <= -0.001) {
                        low = current;
                        if (high == Double.MaxValue) {
                            current = current * 10.0;
                        } else {
                            current = (high + low) / 2.0;
                        }
                    }
                    else {
                        break;
                    }
                }
                GD.Print("Reached equilibrium price: " + current + " for goods: " + goods.Name);
                foreach (Purchase purchase in purchaseList) 
                {
                    purchase.ExecutePurchase(current);
                }
                foreach (Sell sell in sellList)
                {
                    sell.ExecuteSell(current);
                }
                GoodsPrices[goods] = current;
            }
        }
    }

    private double GetPurchaseAmountAtPrice(double price, List<Purchase> purchases)
    {
        double purchaseAmount = 0f;
        foreach (Purchase purchase in purchases) {
            purchaseAmount += purchase.GetPurchaseAmountAtPrice(price);
        }
        return purchaseAmount;
    }

    private double GetSellAmountAtPrice(double price, List<Sell> sells)
    {
        double sellAmount = 0f;
        foreach (Sell sell in sells) {
            sellAmount += sell.GetSellAmountAtPrice(price);
        }
        return sellAmount;
    }

    public void RunProduction()
    {
        foreach (Production production in Productions)
        {
            production.RunProduction();
            production.DistributeCash();
        }   
    }
}
