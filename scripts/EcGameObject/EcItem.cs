using Godot;
using System;
using System.Collections.Generic;

public partial class EcItem : EcGameObject
{
    [Export]
    public string ItemName = "UnknownItem";

    [Export]
    public double Decay = 0.0;

    [Export]
    public bool IsService = false;
}