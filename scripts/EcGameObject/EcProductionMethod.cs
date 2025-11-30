using Godot;
using System;

public partial class EcProductionMethod : EcGameObject
{
    [Export]
    public string ProductionMethodName = "Unnamed Production Method";

    [Export]
    public Godot.Collections.Dictionary<int, double> InputItemAmounts = [];

    [Export]
    public Godot.Collections.Dictionary<int, double> OutputItemAmounts = [];

}