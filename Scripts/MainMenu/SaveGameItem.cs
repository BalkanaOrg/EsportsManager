using EsportsManager.EsportsManager.Data.Models;
using Godot;
using System;
using System.Linq;

public partial class SaveGameItem : PanelContainer
{
    [Export] private Label teamOrgLabel;
    [Export] private Label dateLabel;
    [Export] private Button loadButton;
    [Export] private Button deleteButton;

    public Guid SaveId { get; private set; }
    public event Action<Guid> OnLoadPressed;
    // Called when the node enters the scene tree for the first time.

    public void Initialize(GameState gameState, string displayName)
    {
        SaveId = gameState.Id;
        if(gameState.UserOrgId!=null)
        {
            teamOrgLabel.Text = gameState.Organizations.FirstOrDefault(o => o.Id == gameState.UserOrgId)?.Name.ToString();
        }
        else if (gameState.UserTeamId != null)
        {
            teamOrgLabel.Text = gameState.Teams.FirstOrDefault(t => t.Id == gameState.UserTeamId)?.Name.ToString();
        }
        else
        {
            teamOrgLabel.Text = "No Team/Organization";
        }
        dateLabel.Text = $"Year: {gameState.CurrentYear}, Week: {gameState.CurrentWeek}";

        loadButton.Pressed += () => OnLoadPressed?.Invoke(SaveId);
    }

    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
