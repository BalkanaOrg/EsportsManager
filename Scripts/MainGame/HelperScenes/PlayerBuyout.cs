using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Linq;

public partial class PlayerBuyout : Control
{
	[Export] Button CloseWindowButton;

	[Export] Slider SalarySlider;
	[Export] Slider BuyoutSlider;
	[Export] Slider BonusSlider;
	[Export] Slider PrizePoolSlider;

	[Export] Label SalaryLabel;
    [Export] Label BuyoutLabel;
    [Export] Label BonusLabel;
    [Export] Label PrizePoolLabel;

	[Export] Label PlayerName;
	[Export] Label PlayerAge;
    [Export] Label TeamName;	
    [Export] Label WorldRanking;	

    [Export] Label PlayerAcceptance;	
    [Export] Label TeamAcceptance;

    [Export] OptionButton yearDropdown;
    [Export] OptionButton weekDropdown;

    [Export] CheckBox CanBeBenchedCheckBox;
	[Export] CheckBox CanBeSoldCheckBox;
	[Export] CheckBox MustHaveConsentCheckBox;

	[Export] Button SubmitButton;


	GameDbContext context;
    GameState gameState;
    Player player;
    Team team;
    Team userTeam;
    bool shouldPlayerAccept = false;
    bool shouldTeamAccept = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		context = GameService.LoadDataBaseInformation();
		gameState = GameService.TransferGameState();
        InitialValues();

        InitializeYearDropdown();
        yearDropdown.ItemSelected += OnYearSelected;


        SalaryLabel.Text = SalarySlider.Value.ToString();
        BuyoutLabel.Text = BuyoutSlider.Value.ToString();
        BonusLabel.Text = BonusSlider.Value.ToString();
        PrizePoolLabel.Text = PrizePoolSlider.Value.ToString();

		PlayerName.Text = player.FullName;
        PlayerAge.Text = player.Age.ToString();
        TeamName.Text = team.Name;
        WorldRanking.Text = $"#{team.WorldRanking.ToString()}";


        SalarySlider.ValueChanged += OnValueChangedSalary;
        BuyoutSlider.ValueChanged += OnValueChangedBuyout;
        BonusSlider.ValueChanged += OnValueChangedBonus;
        PrizePoolSlider.ValueChanged += OnValueChangedPrizePool;
        CanBeBenchedCheckBox.Toggled += OnCheckBoxToggled;
        CanBeSoldCheckBox.Toggled += OnCheckBoxToggled;
        MustHaveConsentCheckBox.Toggled += OnCheckBoxToggled;

        SubmitButton.Pressed += SignContract;
    }

    private void WeekDropdown_ItemSelected(long index)
    {
        throw new NotImplementedException();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
    

    private void OnCheckBoxToggled(bool pressed)
    {
        WillPlayerAccept();
        WillTeamAccept();
    }

    private void InitialValues()
    {
        var _userTeam = context.Teams.FirstOrDefault(c => c.Id == gameState.UserTeamId && c.GameStateId == gameState.Id);
        var examplePlayer = context.Players.Where(c => c.GameStateId == gameState.Id && c.TeamId!=_userTeam.Id).OrderByDescending(c => c.Rating).FirstOrDefault();
        userTeam = _userTeam;
        player = examplePlayer;
        team = player.Team;

        SalarySlider.Value = player.ExpectedSalary;
        SalarySlider.MinValue = player.ExpectedSalary * 0.5;
        SalarySlider.MaxValue = userTeam.Budget;

        BuyoutSlider.Value = player.Buyout;
        BuyoutSlider.MinValue = player.Buyout * 0.5;
        BuyoutSlider.MaxValue = player.MarketValue*3;

        BonusSlider.Value = 5000;
        BonusSlider.MinValue = 0;
        BonusSlider.MaxValue = userTeam.Budget/10;

        //Prize pool da se vzemat vsichki stoinosti na vsichki igrachi na otbora i da se ostavi mejdu 0 i kuvto procent e ostanal

    }

    private void InitializeYearDropdown()
    {
        yearDropdown.Clear();

        // Add a default empty option
        yearDropdown.AddItem("Select a year");

        // Add all countries from our dictionary
        for(int year = gameState.CurrentYear; year <= gameState.CurrentYear + 5; year++)
        {
            yearDropdown.AddItem(year.ToString());
        }

        // Set the default selection
        yearDropdown.Selected = 0;
    }

    private void OnYearSelected(long index)
    {
        // Ignore if the default "Select a country" is chosen
        //int week = yearDropdown.Text.ToInt();
        if (index == 0)
        {
            weekDropdown.Clear();
            weekDropdown.Disabled = true;
            return;
        }
        else
        {
            string selectedYear = yearDropdown.GetItemText((int)index);
            weekDropdown.Disabled = false;
            if (index == gameState.CurrentYear)
            {
                for (int i = gameState.CurrentWeek + 1; i <= 52; i++)
                {
                    weekDropdown.AddItem(i.ToString());
                }
            }
            else
            {
                for (int i = 1; i <= 52; i++)
                {
                    weekDropdown.AddItem(i.ToString());
                }
            }
        }
    }

    public void Initialize(Player player)
	{
		this.player = player;
		team = player.Team;
    }

    private void OnValueChangedSalary(double value)
	{
		SalaryLabel.Text = value.ToString();
        WillPlayerAccept();
        WillTeamAccept();

    }
    private void OnValueChangedBonus(double value)
	{
		BonusLabel.Text = value.ToString();
        WillPlayerAccept();
        WillTeamAccept();

    }
    private void OnValueChangedBuyout(double value)
	{
		BuyoutLabel.Text = value.ToString();
        WillPlayerAccept();
        WillTeamAccept();

    }
    private void OnValueChangedPrizePool(double value)
	{
		PrizePoolLabel.Text = value.ToString();
        WillPlayerAccept();
        WillTeamAccept();

    }
    private void WillPlayerAccept()
    {
        double salary = SalarySlider.Value;
        double buyout = BuyoutSlider.Value;
        double bonus = BonusSlider.Value;
        double prizePoolPercentage = PrizePoolSlider.Value;
        bool canBeTraded = CanBeSoldCheckBox.ButtonPressed;
        bool canBeBenched = CanBeBenchedCheckBox.ButtonPressed;
        bool mustHaveConsent = MustHaveConsentCheckBox.ButtonPressed;
        int[] contractExp = new int[] { yearDropdown.Text.ToInt(), weekDropdown.Text.ToInt() };
        shouldPlayerAccept = AIController.ShouldPlayerAcceptContract(userTeam, player, buyout, salary, salary * 0 * 2, bonus, prizePoolPercentage, canBeTraded, canBeBenched, mustHaveConsent, contractExp);
        if (shouldPlayerAccept)
        {
            PlayerAcceptance.Text = "Player will accept the contract.";
            PlayerAcceptance.AddThemeColorOverride("font_color", new Color(0, 1, 0)); // Green
        }
        else
        {
            PlayerAcceptance.Text = "Player will not accept the contract.";
            PlayerAcceptance.AddThemeColorOverride("font_color", new Color(1, 0, 0)); // Red
        }
    }
    private void WillTeamAccept()
    {
        double buyout = BuyoutSlider.Value;
        shouldTeamAccept = AIController.ShouldTeamAcceptBuyout(team, player, userTeam, buyout, shouldPlayerAccept);
        if (shouldTeamAccept)
        {
            TeamAcceptance.Text = "Team will accept the buyout.";
            TeamAcceptance.AddThemeColorOverride("font_color", new Color(0, 1, 0)); // Green
        }
        else
        {
            TeamAcceptance.Text = "Team will not accept the buyout.";
            TeamAcceptance.AddThemeColorOverride("font_color", new Color(1, 0, 0)); // Red
        }
    }
    private void SignContract()
    {
        if(shouldPlayerAccept && shouldTeamAccept)
        {
            ContractService.BuyPlayerFromTeam(userTeam, player, team, BuyoutSlider.Value, BuyoutSlider.Value, SalarySlider.Value, SalarySlider.Value * 0.2, BonusSlider.Value, CanBeSoldCheckBox.ButtonPressed, CanBeBenchedCheckBox.ButtonPressed, MustHaveConsentCheckBox.ButtonPressed, PrizePoolSlider.Value, new int[] { yearDropdown.Text.ToInt(), weekDropdown.Text.ToInt() });
            GD.Print("Player has been transfered to" + userTeam.Name);
        }
        else
        {
            GD.Print("Player or Team did not accept the contract.");
        }
    }
}
