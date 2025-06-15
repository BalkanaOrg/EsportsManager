using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Linq;

public partial class TeamInWorldRanking : Control
{
    [Signal] public delegate void TeamClickedEventHandler(string teamId);
    private string teamId;
    private Team team;

    [Export] PackedScene MainGame;
    [Export] Control OpenedPanelsNode;
    [Export] PackedScene TeamProfile;

	[Export] Label WorldRanking;
	[Export] Label TeamName;
	[Export] Label Region;
	[Export] Label VRS;

	private GameDbContext context;
    private GameState gameState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		context = GameService.LoadDataBaseInformation();
        gameState = GameService.TransferGameState();
        MouseEntered += () => Modulate = new Color(1, 1, 1, 0.8f); // Example: fade on hover
        MouseExited += () => Modulate = new Color(1, 1, 1, 1);
        MouseFilter = MouseFilterEnum.Stop; // 👈 required for click events
        SetDeferred("size_flags_horizontal", (int)SizeFlags.ExpandFill);
        SetProcessInput(true);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent &&
            mouseEvent.Pressed &&
            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Emitting signal for: ", teamId);
            EmitSignal("TeamClicked", teamId);
        }
    }

    //private void OnGuiInput(InputEvent @event)
    //{
    //    if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
    //    {
    //        GD.Print($"Team clicked: {team.Name}");

    //        EmitSignal("TeamClicked", TeamId.ToString());
    //    }
    //}

    private void OpenTeamProfile(Team team)
    {
        //// Option 1: Load and replace current scene
        //var profileInstance = TeamProfile.Instantiate<TeamProfile>();

        //// Replace current UI — depends on your scene tree structure
        //var root = GetTree().Root;
        //var main = root.GetNode("Main"); // adjust to your node name
        //main.AddChild(profileInstance);
        //profileInstance.InstantiateTeam(team);
    }

    public void Initialize(string teamId)
    {
        context = GameService.LoadDataBaseInformation();
        gameState = GameService.TransferGameState();
        var team = context.Teams.FirstOrDefault(c=>c.Id==Guid.Parse(teamId));
        var vrs = context.VRSRankings.FirstOrDefault(c => c.TeamId == team.Id);
        this.team = team;
        this.teamId = teamId;
        WorldRanking.Text = $"#{team.WorldRanking}";
        TeamName.Text = team.Name;
        Region.Text = team.Region;
        VRS.Text = $"{vrs.Points.ToString()} VRS";
    }
}
