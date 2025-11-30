using Godot;
using System;

public partial class EcProduction : EcGameObject
{
    [Export]
    public string ProductionName = "Unnamed Production";

    [Export]
    public Godot.Collections.Array<int> ProductionMethodIds;

    [Export]
    public Godot.Collections.Array<int> InputSectorIds;

    [Export]
    public Godot.Collections.Array<int> OutputSectorIds;
}