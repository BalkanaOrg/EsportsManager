using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;

public partial class UIElements : Control
{
	[Export] Button NextWeekButton;
	[Export] Label PrestiegeLabel;
	[Export] Label BudgetLabel;
    [Export] Label WeeklyExpense;
	[Export] Label CurrentYearAndWeekLabel;


	private GameDbContext context = GameService.LoadDataBaseInformation();
    private GameState gameState = GameService.TransferGameState();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        UpdateUI();
        NextWeekButton.Pressed += NextWeek;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void NextWeek()
	{
		GameService.NextWeek();
        gameState = GameService.TransferGameState();
		UpdateUI();
    }
	private void UpdateUI()
	{
		var userTeam = gameState.Teams.Find(t => t.Id == gameState.UserTeamId);
        PrestiegeLabel.Text = $"Prestige: {userTeam.Prestige}";
        BudgetLabel.Text = $"Budget: ${userTeam.Budget}";
        WeeklyExpense.Text = $"Weekly Expense: ${userTeam.WeeklyExpense}";
        CurrentYearAndWeekLabel.Text = $"Week: {gameState.CurrentWeek}, Year: {gameState.CurrentYear}";
	}
}
