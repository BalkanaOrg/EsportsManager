using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WorldRanking : Control
{
    [Export] Control Panel;
    [Export] PackedScene WorldRankingTeam;
    [Export] PackedScene TeamProfile;

    [Export] OptionButton gameFilter;
	[Export] Label WorldRankingLabel;

	[Export] Button ClosePanelButton;
	[Export] VBoxContainer WorldRankingContainer;

    private GameDbContext context;
    private GameState gameState;

	private List<Game> availableGames = new();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		context = GameService.LoadDataBaseInformation();
		gameState = GameService.TransferGameState();
		LoadGames();
		ClosePanelButton.Pressed += OnCloseButtonPresesed;
        gameFilter.ItemSelected += OnGameSelected;
        //UpdateRankingUI();
        Update();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    private void Update()
    {
        OnGameSelected(0);
    }

	private void LoadGames()
	{
		availableGames = context.Games.Where(c=>c.GameStateId == gameState.Id).ToList();
		if(availableGames != null)
		{
			gameFilter.Clear();
			foreach(var game in availableGames)
			{
				gameFilter.AddItem(game.ShortName);
            }
		}
    }

	private void OnGameSelected(long index)
	{
		//if(index < 0 || index >= availableGames.Count)
  //      {
		//	return;
  //      }
		var selectedGame = availableGames[(int)index];

		FilterRankingByGame(selectedGame);
    }

    private void FilterRankingByGame(Game game)
    {
        var filteredTeams = context.Teams
            .Where(t => t.GameStateId == gameState.Id && t.GameId == game.Id)
            .OrderBy(t => t.WorldRanking)
            .ToList();
        WorldRankingLabel.Text = $"World Ranking for {game.FullName}";
        UpdateRankingUI(filteredTeams);
    }

    public void UpdateRankingUI(List<Team> teams)
    {
        // Clear existing children
        foreach (Node child in WorldRankingContainer.GetChildren())
        {
            child.QueueFree();
        }

        // Add new items
        foreach (var team in teams)
        {
            var item = WorldRankingTeam.Instantiate<TeamInWorldRanking>();
            item.Initialize(team.Id.ToString());
            item.Connect("TeamClicked", new Callable(this, nameof(OnTeamClicked)));
            //item.TeamClicked += OnTeamClicked;
            WorldRankingContainer.AddChild(item);
        }
    }

    private void OnTeamClicked(string teamid)
    {
        GD.Print($"Team clicked: {teamid}");
        var profileInstance = TeamProfile.Instantiate<TeamProfile>();

        // Pass team ID if needed
        if (profileInstance is TeamProfile teamProfile)
            teamProfile.LoadTeam(teamid);

        // Find the OpenedPanels container
        var openedPanels = GetParent().GetNode<Control>("OpenedPanels"); 
        //var main = GetTree().Root.GetNode<Control>("main");
        //var openedPanels = main.GetNode<Control>("OpenedPanels");

        // Optional: Clear old children if single panel is expected
        foreach (Node child in openedPanels.GetChildren())
            child.QueueFree();

        openedPanels.AddChild(profileInstance);
    }

    private void OnCloseButtonPresesed()
	{
        Panel.Visible = false;
    }
}
