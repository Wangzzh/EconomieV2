using Godot;
using System;

public partial class EcProductionOutputSector : EcGameObject
{
    [Export]
    public string SectorName = "Unnamed sector";

    [Export]
    public int CashPoolId = -1;
    
    [Export]
    public int ItemPoolId = -1;

    [Export]
    public int SellOrderId = -1;

    [Export]
    public double UnitAmount = 1.0;

    [Export]
    public string OutputMethod = OUTPUT_METHOD.DO_NOTHING;

    public static class OUTPUT_METHOD
    {
        public static string SELL_UNIT_AMOUNT_CAP_2X = "SELL_UNIT_AMOUNT_CAP_2X";
        public static string DO_NOTHING = "DO_NOTHING";
    }

    public static EcProductionOutputSector CreateOutputSectorForProduction(EcItem item, EcItem currency, string sectorName)
    {
        EcProductionOutputSector sector = new()
        {
            SectorName = sectorName
        };
        sector.StoreAsGameObject();

        // On creation, default to unit amount and 1.0 price
        EcStorage itemPool = new()
        {
            ItemId = item.Id,
            StorageName = sectorName + " / Item Pool"
        };
        sector.ItemPoolId = itemPool.StoreAsGameObject();
        EcStorage cashPool = new ()
        {
            ItemId = currency.Id,
            StorageName = sectorName + " / Cash Pool"
        };
        sector.CashPoolId = cashPool.StoreAsGameObject();
        EcSellOrder order = new ()
        {
            itemId = item.Id,
            ownerOutputSectorId = sector.Id,
            purchaseOrderName = sectorName + " / Sell"
        };
        sector.SellOrderId = order.StoreAsGameObject();
        return sector;
    }

    public void UpdateUnitAmount(double newUnitAmount)
    {
        UnitAmount = Math.Max(newUnitAmount, 0.1);
        EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
        itemPool.UpdateUnitAmount(UnitAmount);
        EcSellOrder sellOrder = GetGameObject<EcSellOrder>(SellOrderId);
        EcStorage cashPool = GetGameObject<EcStorage>(CashPoolId);
        cashPool.UpdateUnitAmount(UnitAmount * Math.Max(0.1, sellOrder.LastPrice));
        RefreshSellOrder();
    }

    public void RefreshSellOrder()
    {
        EcSellOrder sellOrder = GetGameObject<EcSellOrder>(SellOrderId);
        if (OutputMethod == OUTPUT_METHOD.SELL_UNIT_AMOUNT_CAP_2X)
        {
            EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
            sellOrder.MaxAmount = Math.Min(itemPool.GetMaxOutputAmount() * 0.5, itemPool.DesiredUnitAmount * 2.0);
            sellOrder.DesiredAmount = Math.Min(itemPool.GetMaxOutputAmount() * 0.25, itemPool.DesiredUnitAmount);
            sellOrder.Active = true;
        }
        else
        {
            sellOrder.Active = false;
        }
    }

    public void PreMarket()
    {
        RefreshSellOrder();
    }

    public void PostMarket()
    {
        // Modify item and cash amounts
        EcStorage cashPool = GetGameObject<EcStorage>(CashPoolId);
        EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
        EcSellOrder sell = GetGameObject<EcSellOrder>(SellOrderId);
        cashPool.Amount += sell.LastValue;
        itemPool.Amount -= sell.LastAmount;
    }

    public void PostProduction()
    {
        
    }
}