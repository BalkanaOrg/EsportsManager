using EsportsManager.EsportsManager.Data.Models;
using Godot;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public partial class LoadGamePanel : Panel
{
    [Export] private NodePath savesContainerPath;
    [Export] private PackedScene saveGameItemScene;

    private VBoxContainer savesContainer;
    private GameDbContext db;

    public event Action<GameState> OnGameLoaded;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        savesContainer = GetNode<VBoxContainer>(savesContainerPath);
        db = new GameDbContext(ProjectSettings.GlobalizePath("user://esports_manager.db"));
        RefreshSavesList();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void RefreshSavesList()
    {
        // Clear existing items
        foreach (Node child in savesContainer.GetChildren())
        {
            child.QueueFree();
        }

        // Load all game states with related data
        var gameStates = db.GameStates
            .OrderByDescending(gs => gs.CurrentYear)
            .ThenByDescending(gs => gs.CurrentWeek)
            .DefaultIfEmpty()
            .ToList();

        // Create UI items for each save
        foreach (var gameState in gameStates)
        {
            string displayName = GetDisplayName(gameState);
            if (displayName != null)
            {
                var orgs = db.Organizations.Where(o=>o.GameStateId == gameState.Id).ToList();
                var teams = db.Teams.Where(t => t.GameStateId == gameState.Id).ToList();
                gameState.Teams.AddRange(teams);
                gameState.Organizations.AddRange(orgs);
                var item = saveGameItemScene.Instantiate<SaveGameItem>();
                item.Initialize(gameState, displayName);
                item.OnLoadPressed += OnSaveItemPressed;
                savesContainer.AddChild(item);
            }
        }
    }

    private string GetDisplayName(GameState gameState)
    {
        // Try to get organization name first
        if (gameState.UserOrgId != null)
        {
            var org = db.Organizations
                .AsNoTracking()
                .FirstOrDefault(o => o.Id == gameState.UserOrgId);
            return org?.Name ?? "Organization";
        }

        // Fall back to team name
        if (gameState.UserTeamId != null)
        {
            var team = db.Teams
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == gameState.UserTeamId);
            return team?.Name ?? "Team";
        }

        // Fallback if neither exists
        return $"Save {gameState.Id.ToString().Substring(0, 4)}";
    }

    private void OnSaveItemPressed(Guid saveId)
    {
        var gameState = db.GameStates
            .FirstOrDefault(gs => gs.Id == saveId);

        if (gameState != null)
        {
            // Load the appropriate entity (Team or Org)
            if (gameState.UserOrgId != null)
            {
                var org = db.Organizations
                    .Where(t => t.GameStateId == gameState.Id)
                    .ToList();
                gameState.Organizations = org;
            }
            else if (gameState.UserTeamId != null) 
            {
                var team = db.Teams
                    .Where(t => t.GameStateId == gameState.Id)
                    .ToList();
                gameState.Teams = team;
            }

            OnGameLoaded?.Invoke(gameState);
        }
    }
}
