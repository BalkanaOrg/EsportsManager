using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Collections.Generic;

public partial class LocationSelector : Control
{
    [Export] private NodePath optionButtonCountryPath;
    [Export] private NodePath optionButtonCityPath;

    private OptionButton countryDropdown;
    private OptionButton cityDropdown;

    private Dictionary<string, List<string>> countryCityMap = TeamFactory.CountryCities;


    public override void _Ready()
    {
        // Get references to the OptionButtons
        countryDropdown = GetNode<OptionButton>(optionButtonCountryPath);
        cityDropdown = GetNode<OptionButton>(optionButtonCityPath);

        // Initialize the UI
        InitializeCountryDropdown();

        // Connect signals - NEW Godot 4.0 syntax
        countryDropdown.ItemSelected += OnCountrySelected;

        // Disable city dropdown initially
        cityDropdown.Disabled = true;
    }

    private void InitializeCountryDropdown()
    {
        countryDropdown.Clear();

        // Add a default empty option
        countryDropdown.AddItem("Select a country");

        // Add all countries from our dictionary
        foreach (string country in countryCityMap.Keys)
        {
            countryDropdown.AddItem(country);
        }

        // Set the default selection
        countryDropdown.Selected = 0;
    }

    private void OnCountrySelected(long index)
    {
        // Ignore if the default "Select a country" is chosen
        if (index == 0)
        {
            cityDropdown.Clear();
            cityDropdown.Disabled = true;
            return;
        }

        // Get the selected country
        string selectedCountry = countryDropdown.GetItemText((int)index);

        // Populate cities for the selected country
        PopulateCityDropdown(selectedCountry);

        // Enable the city dropdown
        cityDropdown.Disabled = false;
    }

    private void PopulateCityDropdown(string country)
    {
        cityDropdown.Clear();

        if (countryCityMap.TryGetValue(country, out List<string> cities))
        {
            // Add a default empty option
            cityDropdown.AddItem("Select a city");

            foreach (string city in cities)
            {
                cityDropdown.AddItem(city);
            }

            // Set the default selection
            cityDropdown.Selected = 0;
        }
        else
        {
            GD.PrintErr($"No cities found for country: {country}");
            cityDropdown.Disabled = true;
        }
    }
}
