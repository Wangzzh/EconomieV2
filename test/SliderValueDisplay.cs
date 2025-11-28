using Godot;
using System;

public partial class SliderValueDisplay : Label
{
	public void Update(double value) {
		base.Text = value.ToString("F1");
	}
}
