using Godot;
using System;
using System.Collections.Generic;

public partial class EcItem : EcGameObject
{
    [Export]
    public string ItemName = "Unnamed Item";

    [Export]
    public double Decay = 0.0;

    [Export]
    public bool IsService = false;
}