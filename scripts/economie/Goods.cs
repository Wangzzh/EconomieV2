using System;

public partial class Goods(string name, double decay=0.0)
{
	public string Name { get; set; } = name;
	public double Decay = decay;
}
