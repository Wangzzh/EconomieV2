using Godot;
using System;

public partial class EcProductionOutputSector : EcGameObject
{
    [Export]
    public int CashPoolId;
    
    [Export]
    public int ItemPoolId;

    [Export]
    public int SellOrderId;

    [Export]
    public string OutputMethod;

    public static class OUTPUT_METHOD
    {
        public static string SELL_UNIT_AMOUNT = "PURCHASE_UNIT_AMOUNT";
        public static string DO_NOTHING = "DO_NOTHING";
    }

    public void ExecuteSell()
    {
        EcStorage cashPool = GetGameObject<EcStorage>(CashPoolId);
        EcStorage itemPool = GetGameObject<EcStorage>(ItemPoolId);
        EcSellOrder sell = GetGameObject<EcSellOrder>(SellOrderId);
        cashPool.Amount += sell.LastValue;
        itemPool.Amount -= sell.LastAmount;
    }
}