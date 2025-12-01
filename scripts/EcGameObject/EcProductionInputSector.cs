using Godot;
using System;

public partial class EcProductionInputSector : EcGameObject
{
    [Export]
    public string SectorName = "Unnamed sector";

    [Export]
    public int CashPoolId = -1;
    
    [Export]
    public int ItemPoolId = -1;

    [Export]
    public int PurchaseOrderId = -1;

    [Export]
    public double UnitAmount = 1.0;

    [Export]
    public string InputMethod = INPUT_METHOD.DO_NOTHING;

    public static class INPUT_METHOD
    {
        public static string PURCHASE_UNIT_AMOUNT_CAP_2X = "PURCHASE_UNIT_AMOUNT_CAP_2X";
        public static string DO_NOTHING = "DO_NOTHING";
    }

    public static EcProductionInputSector CreateInputSectorForProduction(EcItem item, EcItem currency, string sectorName)
    {
        EcProductionInputSector sector = new()
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
        EcPurchaseOrder order = new ()
        {
            itemId = item.Id,
            ownerInputSectorId = sector.Id,
            purchaseOrderName = sectorName + " / Purchase"
        };
        sector.PurchaseOrderId = order.StoreAsGameObject();
        return sector;
    }

    public void UpdateUnitAmount(double newUnitAmount)
    {
        UnitAmount = Math.Max(newUnitAmount, 0.1);
        EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
        itemPool.UpdateUnitAmount(UnitAmount);
        EcPurchaseOrder purchaseOrder = GetGameObject<EcPurchaseOrder>(PurchaseOrderId);
        EcStorage cashPool = GetGameObject<EcStorage>(CashPoolId);
        cashPool.UpdateUnitAmount(UnitAmount * Math.Max(0.1, purchaseOrder.LastPrice));
        RefreshPurchaseOrder();
    }

    public void RefreshPurchaseOrder()
    {
        if (InputMethod == INPUT_METHOD.PURCHASE_UNIT_AMOUNT_CAP_2X)
        {
            EcPurchaseOrder purchaseOrder = GetGameObject<EcPurchaseOrder>(PurchaseOrderId);
            EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
            purchaseOrder.MaxAmount = itemPool.DesiredUnitAmount * 2.0;
            purchaseOrder.DesiredAmount = itemPool.DesiredUnitAmount;
        }
    }

    public void PreMarket()
    {
    }

    public void PostMarket()
    {
        // Modify item and cash amounts
        EcStorage cashPool = GetGameObject<EcStorage>(CashPoolId);
        EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
        EcPurchaseOrder purchase = GetGameObject<EcPurchaseOrder>(PurchaseOrderId);
        cashPool.Amount -= purchase.LastValue;
        itemPool.Amount += purchase.LastAmount;
    }

    public void PostProduction()
    {
        
    }
}