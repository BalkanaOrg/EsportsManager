using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using EsportsManager.EsportsManager.Data.Models;
using EsportsManager.EsportsManager.Data.Data;
using Microsoft.EntityFrameworkCore;
using EsportsManager.Scripts.MainMenu.Factories;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

public partial class GameManager : Node
{
    private GameDbContext _db;
    private Random random = new Random();
    public GameState gameState;
    [Export] NodePath activationPanelPath;
    [Export] NodePath smallPanelPath;

    Button activationPanel;
    Panel smallPanel;

    [Export] Label teamNameLabel;
    [Export] Button createSeries;
    [Export] Button playSeries;
    [Export] Button signSponsor;

    public override void _Ready()
    {
        activationPanel = GetNode<Button>(activationPanelPath);
        smallPanel = GetNode<Panel>(smallPanelPath);

        gameState = LoadGameState();
        _db = LoadDataBase();
        //gameState = _db.GameStates.Where(c=>c.Id == gameState.Id).Include(gs => gs.Teams).FirstOrDefault();
        //gameState.AllTeams = _db.Teams.Where(t => t.GameStateId == gameState.Id).ToList();
        //gameState.AllGames = _db.Games.Where(g => g.GameStateId == gameState.Id).ToList();
        //gameState.AllTournaments = _db.Tournaments.Where(t => t.GameStateId == gameState.Id).ToList();
        //gameState.Players = _db.Players.Where(p => p.GameStateId == gameState.Id).ToList();
        //gameState.FreeAgents = gameState.Players.Where(p => p.TeamId == null).ToList();
        //gameState.Organizations = _db.Organizations.Where(o => o.GameStateId == gameState.Id).ToList();
        //gameState.Maps = _db.Maps.Where(m => m.GameStateId == gameState.Id).ToList();

        //gameState = _db.GameStates.Where(g=>g.Id == gameState.Id)
        //    .FirstOrDefault();

        // Set derived properties
        //gameState.FreeAgents = gameState.Players.Where(p => p.TeamId == null).ToList();

        var team = gameState.Teams.FirstOrDefault(t => t.Id == gameState.UserTeamId);
        teamNameLabel.Text = team != null ? team.Name.ToString() : "No Team Selected";
        //teamNameLabel.Text = gameState.Id.ToString();

        //_db.Database.EnsureDeleted();

        // Create game
        //Game game = new Game { Id = Guid.NewGuid(), FullName = "Counter-Strike", ShortName = "CS", Description = "First Person Shooter", Genre = "FPS", IconURL = "cs.png", GameState = gameState };
        //gameState.AllGames.Add(game);

        //GameService.InitializeNewGame(gameState, "Sigma United", false, "Bulgaria", "Sofia", game);

        ////_db.GameStates.Update(gameState);
        //_db.SaveChanges();
        //smallPanel.Visible = false;
        //activationPanel.MouseFilter = Control.MouseFilterEnum.Pass;
        activationPanel.Pressed += OnHoverAreaEntered;

        createSeries.Pressed+=CreateSeries;
        signSponsor.Pressed+=SignSponsor;

    }
    private void OnHoverAreaEntered()
    {
        // Just toggle (same as your code, but simplified)
        smallPanel.Visible = !smallPanel.Visible;
    }

    private void CreateSeries()
    {
        var tournament = _db.Tournaments.FirstOrDefault(c=>c.GameStateId==gameState.Id);
        var userTeam = gameState.Teams.OrderBy(c=>c.WorldRanking).FirstOrDefault(t => t.Id != gameState.UserTeamId);
        var team2 = gameState.Teams.OrderBy(c => c.WorldRanking).FirstOrDefault(t => t.Id != gameState.UserTeamId && t.Id!=userTeam.Id);

        TournamentService.CreateSeries(tournament, userTeam, team2, 3, "CS", [2025, 1]);
        var series = _db.Series.Where(c=>c.Tournament.GameStateId==gameState.Id).FirstOrDefault(c => c.TournamentId == tournament.Id);
        TournamentService.PlaySeries_CS(series);
    }

    private void SignSponsor()
    {
        var team = gameState.Teams.FirstOrDefault(c=>c.Id==gameState.UserTeamId);
        var sponsor = _db.Sponsors.FirstOrDefault(c=>c.GameStateId == gameState.Id);
        ContractService.SignSponsor(team, sponsor, [2025,14], 25000,ContractPeriodType.Monthly);
    }

    //private void OnHoverAreaExited()
    //{
    //    smallPanel.Visible = false;
    //}

    public override void _ExitTree()
    {
        _db.Dispose(); // Always dispose when done
    }

    public GameState LoadGameState()
    {
        return GameService.TransferGameState();
    }
    public GameDbContext LoadDataBase()
    {
        return GameService.LoadDataBaseInformation();
    }

    private void SeedIfNeeded()
    {
        if (!_db.Players.Any())
        {
            GD.Print("Seeding initial data...");
            List<Player> players = new List<Player>();
            List<GameProfile> gameProfiles = new List<GameProfile>();

            Game game = new Game { Id = Guid.NewGuid(), FullName = "Counter-Strike", ShortName = "CS", Description = "First Person Shooter", Genre = GameGenre.FPS, IconURL = "cs.png", GameState = gameState };
            Game game2 = new Game { Id = Guid.NewGuid(), FullName = "League of Legends", ShortName = "LoL", Description = "Multiplayer Online Battle Arena", Genre = GameGenre.MOBA, IconURL = "lol.png", GameState = gameState };
            Game game3 = new Game { Id = Guid.NewGuid(), FullName = "Rainbow Six Siege", ShortName = "R6", Description = "Extraction shooter", Genre = GameGenre.FPS, IconURL = "r6.png", GameState = gameState };
            Game game4 = new Game { Id = Guid.NewGuid(), FullName = "StarCraft", ShortName = "SC", Description = "Real-Time Strategy", Genre = GameGenre.RTS, IconURL = "sc.png", GameState = gameState };
            _db.Games.AddRange(game, game2, game3,game4);
            _db.SaveChanges();
            List<Team> teams = new List<Team>();
            teams.Add(TeamFactory.CreateTeam(gameState, game, "Bulgaria", "Sofia"));
            teams.Add(TeamFactory.CreateTeam(gameState, game, "Germany", "Berlin"));
            teams.Add(TeamFactory.CreateTeam(gameState, game, "UK", "London"));
            Organization to = OrgFactory.CreateRandomOrganization(gameState, OrganizationType.TO);
            List<Tournament> tournaments = new List<Tournament>();
            for (int i = 0; i < 3; i++)
            {
                Tournament t = TournamentFactory.CreateTournament(gameState, to, game, TournamentType.StadiumLAN, TournamentTier.S, 1000000, 32, 3, 1, TournamentGroupStageFormat.Swiss, TournamentPlayoffsFormat.SingleElimination);
                tournaments.Add(t);
            }



            _db.Organizations.Add(to);
            _db.SaveChanges();
            _db.Teams.AddRange(teams);
            _db.SaveChanges();
            //_db.Players.AddRange(players);
            _db.SaveChanges();
            _db.Tournaments.AddRange(tournaments);
            //PlayerFactory.GeneratePlayer("Bulgarian", team, _db, game);
            _db.SaveChanges();
            foreach(var team in teams)
            {
                for (int i = 0; i < 5; i++)
                {
                    Player p = PlayerFactory.GeneratePlayer(gameState, team);
                    players.Add(p);
                }
            }
            Player a = PlayerFactory.GeneratePlayer(gameState);
            Player b = PlayerFactory.GeneratePlayer(gameState);
            _db.AddRange(players);
            _db.Add(a);
            _db.Add(b);
            _db.SaveChanges();
            List<GameProfile> profiles = new List<GameProfile>();
            foreach (var player in players)
            {
                profiles.Add(PlayerFactory.GenerateGameProfile(player, game));
            }
            profiles.Add(PlayerFactory.GenerateGameProfile(a, game2));
            _db.GameProfiles.AddRange(profiles);
            _db.SaveChanges();
            GD.Print("Seeding complete.");
        }

    }
}