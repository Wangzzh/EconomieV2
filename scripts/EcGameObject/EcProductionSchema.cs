using Godot;
using System;

public partial class EcProductionSchema : EcGameObject
{
    [Export]
    public string SchemaName = "Unnamed Production Schema";

    [Export]
    public Godot.Collections.Array<int> ProductionMethodIds;
}