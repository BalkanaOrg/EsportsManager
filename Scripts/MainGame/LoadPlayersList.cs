using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static Godot.Projection;
using static System.Runtime.InteropServices.JavaScript.JSType;

public partial class LoadPlayersList : Panel
{
    [Export] private VBoxContainer savesContainerPlayers;
    [Export] private ScrollContainer scrollContainer;
    [Export] private PackedScene playerProfileScene;

    private GameDbContext db;
    private GameState gameState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        savesContainerPlayers.SizeFlagsVertical = Control.SizeFlags.Expand;
        savesContainerPlayers.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        db = GameService.LoadDataBaseInformation();
        gameState = GameService.TransferGameState();
        RefreshPlayerList();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void RefreshPlayerList()
	{
        //savesContainerPlayers.SizeFlagsVertical = Control.SizeFlags.Fill;
        //savesContainerPlayers.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        scrollContainer.SizeFlagsVertical = Control.SizeFlags.Expand;
        scrollContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        gameState = db.GameStates
            .Where(gs => gs.Id == gameState.Id)
            .Include(gs => gs.Teams)
                .ThenInclude(team => team.Players) // Include players for each team
            .Include(gs => gs.AllGames)
            .Include(gs => gs.AllTournaments)
            .Include(gs => gs.Players)
            .Include(gs => gs.Organizations)
                .ThenInclude(org => org.Managers) // Include organization members if needed
            .Include(gs => gs.Maps)
            .AsSplitQuery() // Helps with performance with multiple includes
            .FirstOrDefault();
        foreach (Node child in savesContainerPlayers.GetChildren())
        {
            child.QueueFree();
        }

        var players = gameState.Teams
            .Where(t => t.GameStateId == gameState.Id)
            .SelectMany(t => t.Players)
            .Where(p => p != null)
            .OrderByDescending(p => p.Rating)
            .ToList();
        //int i = 0;
        //int a = 0;
        //List<HBoxContainer> containers = new List<HBoxContainer>();
        //foreach (var player in players)
        //{
        //    if(i %6 == 0)
        //    {
        //        a++;
        //    }
        //    var item = playerProfileScene.Instantiate<PlayerProfile>();
        //    item.SetPlayer(player);
        //    //savesContainerPlayers.AddChild(item);
        //    containers[a].AddChild(item);
        //    // Ensure profiles have proper size flags
        //    //savesContainerPlayers.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        //    //savesContainerPlayers.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin;
        //}
        //savesContainerPlayers.AddChild(new HBoxContainer {  });

        int itemsPerRow = 6;
        HBoxContainer currentRow = null;

        foreach (var player in players)
        {
            // Create new row container when needed
            if (currentRow == null || currentRow.GetChildCount() >= itemsPerRow)
            {
                currentRow = new HBoxContainer();

                // Configure the row container
                currentRow.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                currentRow.SizeFlagsVertical = Control.SizeFlags.ShrinkBegin;
                currentRow.Alignment = BoxContainer.AlignmentMode.Center;
                currentRow.MouseFilter = Control.MouseFilterEnum.Pass;
                currentRow.AddThemeConstantOverride("separation", 20); // Space between items

                savesContainerPlayers.AddChild(currentRow);
            }

            // Instantiate and configure player profile
            var item = playerProfileScene.Instantiate<PlayerProfile>();
            item.SetPlayer(player);

            // Set item sizing
            item.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            item.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin;
            item.CustomMinimumSize = new Vector2(190, 260); // Set minimum size

            currentRow.AddChild(item);
        }
        //savesContainerPlayers.QueueSort();
    }
    private void UpdateScrollLayout()
    {
        // This ensures the container calculates its proper size
        savesContainerPlayers.QueueSort();
        savesContainerPlayers.NotifyPropertyListChanged();
        // If using Godot 4+, you might need this:
        savesContainerPlayers.ResetSize();
    }
}
