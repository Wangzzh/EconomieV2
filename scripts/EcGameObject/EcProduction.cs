using Godot;
using System;
using System.Collections.Generic;

public partial class EcProduction : EcGameObject
{
    [Export]
    public string ProductionName = "Unnamed Production";

    [Export]
    public Godot.Collections.Array<int> ProductionMethodIds = [];

    [Export]
    public Godot.Collections.Array<Godot.Collections.Array<int>> InputSectorIds = [];

    [Export]
    public Godot.Collections.Array<Godot.Collections.Array<int>> OutputSectorIds = [];

    [Export]
    public Godot.Collections.Array<double> MaxEfficiency = [];

    // Create a production based on a production schema
    public static EcProduction CreateProductionFromSchema(EcProductionSchema schema, EcItem currency)
    {
        EcProduction production = new()
        {
            ProductionMethodIds = schema.ProductionMethodIds,
            ProductionName = schema.SchemaName
        };
        production.StoreAsGameObject();
        foreach (int productionMethodId in schema.ProductionMethodIds)
        {
            Godot.Collections.Array<int> inputSectorIds = [];
            EcProductionMethod productionMethod = GetGameObject<EcProductionMethod>(productionMethodId);
            foreach (KeyValuePair<int, double> itemAmountPair in productionMethod.InputItemAmounts)
            {
                EcItem item = GetGameObject<EcItem>(itemAmountPair.Key);
                string sectorName = production.ProductionName + " / " + productionMethod.ProductionMethodName + " / " + item.ItemName + " Input";
                EcProductionInputSector inputSector = EcProductionInputSector.CreateInputSectorForProduction(item, currency, sectorName);
                inputSectorIds.Add(inputSector.Id);
            }
            production.InputSectorIds.Add(inputSectorIds);

            Godot.Collections.Array<int> outputSectorIds = [];
            foreach (KeyValuePair<int, double> itemAmountPair in productionMethod.OutputItemAmounts)
            {
                EcItem item = GetGameObject<EcItem>(itemAmountPair.Key);
                string sectorName = production.ProductionName + " / " + productionMethod.ProductionMethodName + " / " + item.ItemName + " Output";
                EcProductionOutputSector outputSector = EcProductionOutputSector.CreateOutputSectorForProduction(item, currency, sectorName);
                outputSectorIds.Add(outputSector.Id);
            }
            production.OutputSectorIds.Add(outputSectorIds);

            production.MaxEfficiency.Add(0.0);
        }
        return production;
    }
    

    public void PreMarket()
    {
        for (int i = 0; i < ProductionMethodIds.Count; i++) {
            foreach(int id in InputSectorIds[i])
            {
                EcProductionInputSector inputSector = GetGameObject<EcProductionInputSector>(id);
                inputSector.PreMarket();
            }
            foreach(int id in OutputSectorIds[i])
            {
                EcProductionOutputSector outputSector = GetGameObject<EcProductionOutputSector>(id);
                outputSector.PreMarket();
            } 
        }
    }

    public void PostMarket()
    {
        for (int i = 0; i < ProductionMethodIds.Count; i++) {
            foreach(int id in InputSectorIds[i])
            {
                EcProductionInputSector inputSector = GetGameObject<EcProductionInputSector>(id);
                inputSector.PostMarket();
            }
            foreach(int id in OutputSectorIds[i])
            {
                EcProductionOutputSector outputSector = GetGameObject<EcProductionOutputSector>(id);
                outputSector.PostMarket();
            } 
        }
    }

    public void RunProduction()
    {
        
    }

    public void PostProduction()
    {
        
    }
}