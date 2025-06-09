using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Linq;

public partial class ShowAllPlayers : ScrollContainer
{
    [Export] private NodePath savesContainerPath;
    [Export] private PackedScene saveGameItemScene;

    private VBoxContainer savesContainer;
    private GameDbContext db;
    private GameState gameState = GameService.TransferGameState();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        savesContainer = GetNode<VBoxContainer>(savesContainerPath);
        RefreshPlayersList();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void RefreshPlayersList()
    {
        // Clear existing items
        foreach (Node child in savesContainer.GetChildren())
        {
            child.QueueFree();
        }

        // Load all game states with related data
        var players = gameState.Players.OrderBy(p => p.Rating)
            .ToList();

        // Create UI items for each save
        foreach (var player in players)
        {
            var item = saveGameItemScene.Instantiate<TeamLabels>();
            string s = $"{player.Nickname} - {player.Age} - {player.ExpectedSalary}";
            item.Initialize(s);
            savesContainer.AddChild(item);
        }
    }

}
