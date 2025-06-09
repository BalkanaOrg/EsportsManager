using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.Scripts.MainMenu.Factories;
using Godot;
using System;
using System.Collections.Generic;

public partial class MainMenu : Control
{
    private GameDbContext _db;
    private Random random = new Random();
    public GameState gameState = new GameState();

    [Export] private NodePath startGameButtonPath;
    [Export] private NodePath loadGameButtonPath;
    [Export] private NodePath optionsButtonPath;
    [Export] private NodePath exitGameButtonPath;

    [Export] private NodePath startGamePanelCloseButtonPath;
    [Export] private NodePath loadGamePanelCloseButtonPath;
    [Export] private NodePath optionPanelCloseButtonPath;

    [Export] private NodePath startGamePanelButtonPath;
    [Export] private NodePath loadGamePanelButtonPath;

    [Export] private NodePath startGamePanelPath;
    [Export] private NodePath loadGamePanelPath;
    [Export] private NodePath optionsPanelPath;

    [Export] private NodePath userNamePath;

    [Export] private OptionButton country;
    [Export] private OptionButton city;

    // UI Elements
    private Button startGameButton;
    private Button loadGameButton;
    private Button optionsButton;
    private Button exitGameButton;

    private Button startGamePanelCloseButton;
    private Button loadGamePanelCloseButton;
    private Button optionPanelCloseButton;

    private Button startGamePanelButton;
    private Button loadGamePanelButton;

    private Panel startGamePanel;
    private LoadGamePanel loadGamePanel;
    private Panel optionsPanel;
    private TextEdit nameField;


    public int SelectedMode { get; private set; } = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        startGameButton = GetNode<Button>(startGameButtonPath);
        loadGameButton = GetNode<Button>(loadGameButtonPath);
        optionsButton = GetNode<Button>(optionsButtonPath);
        exitGameButton = GetNode<Button>(exitGameButtonPath);

        startGamePanelCloseButton = GetNode<Button>(startGamePanelCloseButtonPath);
        loadGamePanelCloseButton = GetNode<Button>(loadGamePanelCloseButtonPath);
        optionPanelCloseButton = GetNode<Button>(optionPanelCloseButtonPath);

        startGamePanelButton = GetNode<Button>(startGamePanelButtonPath);
        loadGamePanelButton = GetNode<Button>(loadGamePanelButtonPath);

        startGamePanel = GetNode<Panel>(startGamePanelPath);
        //loadGamePanel = GetNode<Panel>(loadGamePanelPath);
        loadGamePanel = GetNode<LoadGamePanel>(loadGamePanelPath);
        optionsPanel = GetNode<Panel>(optionsPanelPath);

        nameField = GetNode<TextEdit>(userNamePath);

        startGameButton.Pressed += StartNewGame;
        startGamePanelCloseButton.Pressed += CloseAllPanels;
        loadGamePanelCloseButton.Pressed += CloseAllPanels;
        optionPanelCloseButton.Pressed += CloseAllPanels;
        startGamePanelButton.Pressed += OpenStartGamePanel;
        loadGamePanelButton.Pressed += OpenLoadGamePanel;
        optionsButton.Pressed += OpenOptionsPanel;
        exitGameButton.Pressed += OnExitButtonPressed;

        loadGamePanel.OnGameLoaded += OnGameLoaded;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnGameLoaded(GameState gameState)
    {
        GameService.GenerateDataBase();
        _db = GameService.LoadDataBaseInformation();
        // Hide the load panel
        loadGamePanel.Visible = false;

        // Initialize your game with the loaded state
        //GameService.LoadExistingGame(gameState);
        GameService.SaveGameState(gameState);
        // Transition to main game scene
        GetTree().ChangeSceneToFile("res://main.tscn");
    }

    // Called when a button is pressed (Team or Org)
    public void OnSelectionButtonPressed(int selectedMode)
    {
        SelectedMode = selectedMode;
        GD.Print("Selected mode: ", (selectedMode == 0) ? "Team" : "Org");
    }

    public void OpenStartGamePanel()
    {
        startGamePanel.Visible = true;
        loadGamePanel.Visible = false;
        optionsPanel.Visible = false;
    }

    public void OpenLoadGamePanel()
    {
        startGamePanel.Visible = false;
        loadGamePanel.Visible = true;
        optionsPanel.Visible = false;
    }
    
    public void OpenOptionsPanel()
    {
        startGamePanel.Visible = false;
        loadGamePanel.Visible = false;
        optionsPanel.Visible = true;
    }

    public void CloseAllPanels()
    {
        startGamePanel.Visible = false;
        loadGamePanel.Visible = false;
        optionsPanel.Visible = false;
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }

    public void StartNewGame()
    {
        //string dbPath = ProjectSettings.GlobalizePath("user://esports_manager.db");
        //_db = new GameDbContext(dbPath);
        GameService.GenerateDataBase();
        _db = GameService.LoadDataBaseInformation();
        GameService.SaveGameState(gameState);
        //_db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();
        GenerateInitialGames();

        _db.GameStates.Add(gameState);
        bool s;
        if (SelectedMode == 0) s = false;
        else s = true;
        bool success = false;
        var cs = gameState.AllGames.Find(g => g.ShortName == "CS");
        if (nameField.Text !=string.Empty)
        {
            if (country.Text != string.Empty)
            {
                if (city.Text != string.Empty)
                {
                    GameService.InitializeNewGame(gameState, nameField.Text, s, country.Text, city.Text, cs);
                    success = true;
                }
            }
            //GameService.InitializeNewGame(gameState,nameField.Text, s, "Bulgaria", "Sofia",cs);
        }
        if(success)
        {
            _db.SaveChanges();
            GameService.SaveDataBaseInformation(_db);
            GameService.UpdateTeamRankingsInitially();
            _db.SaveChanges();
            GetTree().ChangeSceneToFile("res://main.tscn");

        }
    }
    public void GenerateInitialGames()
    {
        //List<Game> games = new List<Game>();
        //Game game = new Game { Id = Guid.NewGuid(), FullName = "Counter-Strike", ShortName = "CS", Description = "First Person Shooter", Genre = GameGenre.FPS, IconURL = "cs.png", GameState = gameState };
        //Game game2 = new Game { Id = Guid.NewGuid(), FullName = "League of Legends", ShortName = "LoL", Description = "Multiplayer Online Battle Arena", Genre = GameGenre.MOBA, IconURL = "lol.png", GameState = gameState };
        //Game game3 = new Game { Id = Guid.NewGuid(), FullName = "Rainbow Six Siege", ShortName = "R6", Description = "Extraction shooter", Genre = GameGenre.FPS, IconURL = "r6.png", GameState = gameState };
        //Game game4 = new Game { Id = Guid.NewGuid(), FullName = "StarCraft", ShortName = "SC", Description = "Real-Time Strategy", Genre = GameGenre.RTS, IconURL = "sc.png", GameState = gameState };
        //games.Add(game);
        //games.Add(game2);
        //games.Add(game3);
        //games.Add(game4);
        //gameState.AllGames.AddRange(games);
        GameService.SetupDevelopersAndGames();
    }
}
