using EsportsManager.EsportsManager.Data.Models;
using Godot;
using System;

public partial class TeamLabels : Label
{
    [Export] private Label username;
    // Called when the node enters the scene tree for the first time.

    public void Initialize(string name)
    {
        username.Text = name;
    }

    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
