using Godot;
using System;

public partial class ScrollContainer : Godot.ScrollContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//public override void _UnhandledInput(InputEvent @event)
	//{
	//	if (@event is InputEventMouseButton mbe)
	//	{
	//		if (mbe.ButtonIndex == MouseButton.WheelUp)
	//			GD.Print("Scroll UP detected");
	//		if (mbe.ButtonIndex == MouseButton.WheelDown)
	//			GD.Print("Scroll DOWN detected");
	//	}
	//}
}
