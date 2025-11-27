using Godot;
using System;
using System.Collections.Generic;

public partial class Location {
    public List<Production> Productions = new List<Production>();
    public List<Goods> GoodsList = new List<Goods>();

    public Dictionary<Goods, List<Purchase>> AllGoodsPurchases = new Dictionary<Goods, List<Purchase>>();
    public Dictionary<Goods, List<Sell>> AllGoodsSells = new Dictionary<Goods, List<Sell>>();
    public Dictionary<Goods, double> GoodsPrices = new Dictionary<Goods, double>();

    public void CollectPurchasesAndSells() {
        AllGoodsPurchases.Clear();
        AllGoodsSells.Clear();
        foreach(Goods goods in GoodsList) {
            List<Purchase> purchaseList = new List<Purchase>();
            List<Sell> sellList = new List<Sell>();
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
                double defaultPrice = 1.0;
                foreach (Purchase purchase in purchaseList) {
                    purchase.PurchasedAmount = 0f;
                    purchase.PurchasedValue = 0f;
                    purchase.LastPrice = defaultPrice;
                }
                foreach (Sell sell in sellList) {
                    sell.SoldAmount = 0f;
                    sell.SoldValue = 0f;
                    sell.LastPrice = defaultPrice;
                }
                GoodsPrices[goods] = defaultPrice;
            }
            else {
                double low = 0.0;
                double current = 1.0;
                double high = Double.MaxValue;
                while (high - low >= 0.001) {
                    double surplus = surplusAtPrice(current, purchaseList, sellList);
                    if (surplus >= 0.001) {
                        high = current;
                        current = (high + low) / 2.0;
                    }
                    else if (surplus <= -0.001) {
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
                foreach (Purchase purchase in purchaseList) {
                    purchase.PurchasedAmount = purchase.GetPurchaseAmountAtPrice(current);
                    purchase.PurchasedValue = purchase.PurchasedAmount * current;
                    purchase.LastPrice = current;
                }
                foreach (Sell sell in sellList) {
                    sell.SoldAmount = sell.GetSellAmountAtPrice(current);
                    sell.SoldValue = sell.SoldAmount * current;
                    sell.LastPrice = current;
                }
                GoodsPrices[goods] = current;
            }
        }
    }

    // Returns sell amount - purchase amount
    public double surplusAtPrice(double price, List<Purchase> purchases, List<Sell> sells) {
        double purchaseAmount = 0f;
        double sellAmount = 0f;
        foreach (Purchase purchase in purchases) {
            purchaseAmount += purchase.GetPurchaseAmountAtPrice(price);
        }
        foreach (Sell sell in sells) {
            sellAmount += sell.GetSellAmountAtPrice(price);
        }
        double surplus = sellAmount - purchaseAmount;
        GD.Print("Surplus at price " + price + " is: " + surplus);
        return surplus;
    }
}
