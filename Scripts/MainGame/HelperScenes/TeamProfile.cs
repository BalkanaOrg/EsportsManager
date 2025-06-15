using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TeamProfile : Control
{
	[Export] Label TeamName;
	[Export] Label WorldRanking;
	[Export] Label Region;
	[Export] Label Game;

    [Export] Button ButtonClose;

    [Export] HBoxContainer TeamMainPlayers;
    [Export] PackedScene playerProfileScene;

    private GameDbContext context;
    private GameState gameState;
    private Team team;
    private Game game;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        context = GameService.LoadDataBaseInformation();
        gameState = GameService.TransferGameState();
        ButtonClose.Pressed += OnCloseButtonPresesed;


        UpdateUI();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void InstantiateTeam(string teamid)
    {
        context = GameService.LoadDataBaseInformation();
        gameState = GameService.TransferGameState();
        GD.Print("Instantiating team with ID: ", teamid);
        Guid id = Guid.Parse(teamid);
        var team = context.Teams.Where(c=>c.Id==id).FirstOrDefault();
        if (team == null)
        {
            GD.PrintErr("Team is null");
            return;
        }
        this.team = team;
        TeamName.Text = team.Name;
        WorldRanking.Text = $"#{team.WorldRanking}";
        //Region.Text = team.Region.ToString();

        var game = context.Games.Where(c=>c.Id == team.GameId).FirstOrDefault();
        this.game = game;
        if (game != null)
        {
            //Game.Text = game.ShortName;
        }
        else
        {
            Game.Text = "Unknown Game";
        }

    }
    private void UpdateUI()
    {
        TeamName.Text = team.Name;
        WorldRanking.Text = $"#{team.WorldRanking}";
        Region.Text = team.Region;
        Game.Text = game.ShortName;
        LoadPlayers();
    }

    public void LoadTeam(string teamid)
    {
        GD.Print("Loading profile for team ID: ", teamid);
        InstantiateTeam(teamid);
        UpdateUI();
    }

    private void LoadPlayers()
    {
        var players = gameState.Teams
            .Where(t => t.GameStateId == gameState.Id && t.Id==team.Id)
            .SelectMany(t => t.Players)
            .Where(p => p != null)
            .OrderByDescending(p => p.Rating)
            .ToList();

        foreach (Node child in TeamMainPlayers.GetChildren())
        {
            child.QueueFree();
        }
        foreach (var player in players)
        {
            var item = playerProfileScene.Instantiate<PlayerProfile>();
            item.SetPlayer(player);

            // Set item sizing
            //item.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            //item.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin;
            //item.CustomMinimumSize = new Vector2(190, 260); // Set minimum size
            TeamMainPlayers.AddChild(item);
        }
    }

    private void OnCloseButtonPresesed()
    {
        QueueFree();
    }
}
