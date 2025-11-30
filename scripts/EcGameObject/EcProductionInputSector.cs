using Godot;
using System;

public partial class EcProductionInputSector : EcGameObject
{
    [Export]
    public int CashPoolId;
    
    [Export]
    public int ItemPoolId;

    [Export]
    public int PurchaseOrderId;

    [Export]
    public string InputMethod;

    public static class INPUT_METHOD
    {
        public static string PURCHASE_UNIT_AMOUNT = "PURCHASE_UNIT_AMOUNT";
        public static string DO_NOTHING = "DO_NOTHING";
    }

    public void ExecutePurchase()
    {
        EcStorage cashPool = GetGameObject<EcStorage>(CashPoolId);
        EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
        EcPurchaseOrder purchase = GetGameObject<EcPurchaseOrder>(PurchaseOrderId);
        cashPool.Amount -= purchase.LastValue;
        itemPool.Amount += purchase.LastAmount;
    }
}