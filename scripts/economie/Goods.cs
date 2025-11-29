using System;

public partial class Goods(string name, double decay=0.0, bool service=false)
{
	public string Name { get; set; } = name;
	public double Decay = decay;
	public bool Service = service;
}
