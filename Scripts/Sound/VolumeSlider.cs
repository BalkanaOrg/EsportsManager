using Godot;
using System;

public partial class VolumeSlider : HSlider
{
	[Export] public string bus_name;
	int bus_index;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		bus_index = AudioServer.GetBusIndex(bus_name);
        Value = DbToLinear(AudioServer.GetBusVolumeDb(bus_index));

        ValueChanged += OnVolumeChanged;
    }

    private void OnVolumeChanged(double value)
    {
        // Convert linear slider value (0-1) to decibels
        float dbValue = LinearToDb((float)value);
        AudioServer.SetBusVolumeDb(bus_index, dbValue);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
    private float DbToLinear(float db)
    {
        return Mathf.Pow(10.0f, db / 20.0f);
    }

    private float LinearToDb(float linear)
    {
        return linear > 0 ? 20.0f * Mathf.Log(linear) : -80.0f; // -80dB is effectively silent
    }
}
