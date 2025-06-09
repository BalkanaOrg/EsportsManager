using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public partial class TestPanel : Panel
{
    [Export] private NodePath savesContainerPathPlayers;
    [Export] private PackedScene saveGameItemScenePlayers;

    [Export] private NodePath savesContainerPathTeams;
    [Export] private PackedScene saveGameItemSceneTeams;

    private VBoxContainer savesContainerPlayers;
    private VBoxContainer savesContainerTeams;
    private GameDbContext db;
    private GameState gameState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameState=LoadGameState();
        db = GameService.LoadDataBaseInformation();
        savesContainerPlayers = GetNode<VBoxContainer>(savesContainerPathPlayers);
        savesContainerTeams = GetNode<VBoxContainer>(savesContainerPathTeams);
        RefreshPlayersList();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public void RefreshPlayersList()
    {
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
        GameService.SaveGameState(gameState);
        // Clear existing items
        foreach (Node child in savesContainerPlayers.GetChildren())
        {
            child.QueueFree();
        }
        foreach (Node child in savesContainerTeams.GetChildren())
        {
            child.QueueFree();
        }
        //var _players = db.Players.ToList();
        //gameState.Players.AddRange(_players);
        // Load all game states with related data
        var teams = gameState.Teams.OrderBy(p => p.Name)
            .ToList();
        //var players = db.Teams.SelectMany(t => t.Players).Where(c=>c.GameStateId == gameState.Id)
        //    .OrderBy(p => p.Nickname)
        //    .ToList();
        var players = gameState.Teams
            .Where(t => t.GameStateId == gameState.Id)
            .SelectMany(t => t.Players)
            .Where(p => p != null)
            .OrderBy(p => p.Nickname)
            .ToList();
        foreach (var player in players)
        {
            var item = saveGameItemScenePlayers.Instantiate<TeamLabels>();
            string s = $"{player.Nickname} - {player.Age} - {player.ExpectedSalary}";
            item.Initialize(s);
            savesContainerPlayers.AddChild(item);
        }

        foreach (var team in teams)
        {
            var item = saveGameItemSceneTeams.Instantiate<TeamLabels>();
            string s = $"#{team.WorldRanking} | {team.Tag} | {team.Name}";
            item.Initialize(s);
            savesContainerTeams.AddChild(item);
        }
    }
    public GameState LoadGameState()
    {
        return GameService.TransferGameState();
    }
}
