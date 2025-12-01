using Godot;
using System;

public partial class EcProductionOutputSector : EcGameObject
{
    [Export]
    public string SectorName = "Unnamed sector";

    [Export]
    public int CashPoolId;
    
    [Export]
    public int ItemPoolId;

    [Export]
    public int SellOrderId;

    [Export]
    public string OutputMethod = OUTPUT_METHOD.DO_NOTHING;

    public static class OUTPUT_METHOD
    {
        public static string SELL_UNIT_AMOUNT = "PURCHASE_UNIT_AMOUNT";
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

    public void PreMarket()
    {
        
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