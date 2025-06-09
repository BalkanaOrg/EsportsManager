using EsportsManager.EsportsManager.Data.Models;
using Godot;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EsportsManager.EsportsManager.Data.Models.Tournament;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class GameService
    {
        private static GameState gameState;
        private static GameDbContext context;
        private static Random random = new Random();
        private static List<string> nationalities = PlayerFactory.NationalityData.Keys.ToList();

        private static readonly List<string> _EU = new()
        {
            "Germany", "Bulgaria", "UK", "France", "Spain", "Italy", "Sweden", "Denmark", "Norway", "Finland",
            "Russia", "Ukraine", "Poland", "Turkey", "Netherlands", "Kazakhstan", "Serbia", "Romania", "Greece", "Albania", "Kosovo"
        };

        private static readonly List<string> _AM = new()
        {
            "Brazil", "USA", "Canada", "Mexico", "Argentina", "Columbia"
        };

        private static readonly List<string> _AS = new()
        {
            "China", "South Korea", "Australia", "South Africa", "India", "Japan", "Mongolia", "Vietnam", "Indonesia", "Philippines"
        };

        public static void InitializeNewGame(GameState gs, string name, bool isOrganization, string country, string city, Game game)
        {
            //gameState = new GameState();
            gameState = gs;
            gameState.PlayedYears.Add(gameState.CurrentYear);
            if (isOrganization)
            {
                Organization userOrg = OrgFactory.CreateTeamOrganization(gameState, name, country, city, gameState.CurrentYear, 500000);
                userOrg.isUserControlled = true;
                gameState.UserTeamId = userOrg.Id;
                gameState.Organizations.Add(userOrg);
            }
            else
            {
                Team userTeam = TeamFactory.CreateUserTeam(gameState, game, name,TeamFactory.GenerateTeamTag(name), country, city, gameState.CurrentYear, 500000);
                gameState.UserTeamId = userTeam.Id;
                userTeam.isUserControlled = true;
                gameState.Teams.Add(userTeam);
            }
            SetupCounterStrike();
            GenerateBasicSponsors();
        }

        public static List<GameState> LoadExistingGameChoices(GameDbContext context)
        {
            List<GameState> gameStates = context.GameStates.Select(gs => gs).ToList();
            return gameStates;
        }

        public static void SaveGameState(GameState _gameState)
        {
            gameState = _gameState;
        }

        public static GameState TransferGameState()
        {
            return gameState;
        }

        public static void GenerateDataBase()
        {
            string dbPath = ProjectSettings.GlobalizePath("user://esports_manager.db");
            context = new GameDbContext(dbPath);
        }

        public static void SaveDataBaseInformation(GameDbContext _context)
        {
            context = _context;
        }

        public static GameDbContext LoadDataBaseInformation()
        {
            return context;
        }

        public static void GenerateBasicSponsors()
        {
            List<Sponsorship> s = SponsorFactory.GenerateBasicSponsors();
            gameState.Sponsors.AddRange(s);
        }

        public static void InitializeInitialGame()
        {
            SetupCounterStrike();
        }

        public static void SetupCounterStrike()
        {
            List<Organization> organizations = new List<Organization>();
            List<Organization> TOs = new List<Organization>();
            List<Team> teams = new List<Team>();
            List<GameProfile> profiles = new List<GameProfile>();

            Game game = gameState.AllGames.FirstOrDefault(g => g.ShortName == "CS");

            for (int i = 0; i < 50; i++)
            {
                organizations.Add(OrgFactory.CreateRandomOrganization(gameState, OrganizationType.Team));
            }
            for (int i = 0; i < 10; i++)
            {
                TOs.Add(OrgFactory.CreateRandomOrganization(gameState, OrganizationType.TO));
            }
            gameState.Organizations.AddRange(organizations);
            gameState.Organizations.AddRange(TOs);
            foreach (var org in organizations)
            {
                teams.Add(TeamFactory.CreateTeam(org, game, null));
            }
            for (int i = 0; i < 10; i++)
            {
                teams.Add(TeamFactory.CreateTeam(gameState, game, null, null));
            }
            gameState.Teams.AddRange(teams);
            foreach (var team in teams)
            {
                for (int i = 0; i < 5; i++)
                {
                    Player player = PlayerFactory.GeneratePlayer(gameState, team);
                    team.Players.Add(player);
                    gameState.Players.Add(player);

                    var profile = PlayerFactory.GenerateGameProfile(player, game);
                    player.GameProfiles.Add(profile);
                    player.Rating = PlayerFactory.CalculateCSPlayerRating(player, (GameProfile_CS)profile);
                    player.MarketValue = PlayerFactory.CalculateMarketValue(player,(GameProfile_CS)profile);
                    player.ExpectedSalary = Math.Floor(player.MarketValue/20);
                }
                if (random.Next(20) == 1)
                {
                    Player player = PlayerFactory.GeneratePlayer(gameState, team);
                    team.Bench.Add(player);
                    gameState.Players.Add(player);

                    var profile = PlayerFactory.GenerateGameProfile(player, game);
                    player.GameProfiles.Add(profile);
                }
            }
            GenerateCSMaps(game);
            GenerateTournamentsForYear(TOs, game);
            //UpdateTeamRankings();
        }
        public static void SetupLeagueOfLegends()
        {

        }

        public static void SetupDevelopersAndGames()
        {
            Organization dev = GameFactory.CreateDeveloper(null, null, null, null);
            gameState.Organizations.Add(dev);
            var game = GameFactory.CreateGame(dev, "Counter-Strike", "CS", "ikonka", GameGenre.FPS, "FPS GAME LMAO", null);
            gameState.AllGames.Add(game);

            //for (int i = 0; i < 6; i++)
            //{
            //    var randomDev = GameFactory.CreateDeveloper(null, null, null, null);
            //    gameState.Organizations.Add(dev);
            //    //var randomGame = GameFactory.CreateGame(randomDev,
            //    //gameState.AllGames.Add(randomGame);
            //}
        }

        public static void GenerateTournamentsForYear(List<Organization> TOs, Game game)
        {
            List<Tournament> tournaments = new List<Tournament>();
            tournaments.Add(TournamentFactory.CreateCSMajor(gameState, TOs[random.Next(TOs.Count)], game, 15));
            tournaments.Add(TournamentFactory.CreateCSMajor(gameState, TOs[random.Next(TOs.Count)], game, 45));
            for (int i = 0; i < 10; i++)
            {
                tournaments.Add(TournamentFactory.CreateTournament(gameState, TOs[random.Next(TOs.Count)], game, TournamentType.StadiumLAN, TournamentTier.S, 1000000, 32, 3, 1, TournamentGroupStageFormat.Swiss, TournamentPlayoffsFormat.SingleElimination));
            }
            for (int i = 0; i < 5; i++)
            {
                tournaments.Add(TournamentFactory.CreateTournament(gameState, TOs[random.Next(TOs.Count)], game, TournamentType.StudioLAN, TournamentTier.A, 500000, 32, 3, 1, TournamentGroupStageFormat.Swiss, TournamentPlayoffsFormat.SingleElimination));
            }
            for (int i = 0; i < 10; i++)
            {
                tournaments.Add(TournamentFactory.CreateTournament(gameState, TOs[random.Next(TOs.Count)], game, TournamentType.Online, TournamentTier.D, 10000, 32, 3, 1, TournamentGroupStageFormat.Swiss, TournamentPlayoffsFormat.SingleElimination));
            }
            gameState.AllTournaments.AddRange(tournaments);
        }

        public static void GenerateCSMaps(Game game)
        {
            List<Map> maps = new List<Map>
            {
                new Map {Name = "de_dust2", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_mirage", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_anubis", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_ancient", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_inferno", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_nuke", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_train", ImageUrl = "snimka", Game = game, isActive = true, GameState = gameState },
                new Map {Name = "de_overpass", ImageUrl = "snimka", Game = game, isActive = false, GameState = gameState },
                new Map {Name = "de_cache", ImageUrl = "snimka", Game = game, isActive = false, GameState = gameState },
                new Map {Name = "de_cbbl", ImageUrl = "snimka", Game = game, isActive = false, GameState = gameState },
                new Map {Name = "de_vertigo", ImageUrl = "snimka", Game = game, isActive = false, GameState = gameState },
            };
            gameState.Maps.AddRange(maps);
        }
        public static void NextWeek()
        {
            gameState.CurrentWeek++;
            if(gameState.CurrentWeek > 52)
            {
                gameState.CurrentWeek = 1;
                gameState.CurrentYear++;

                gameState.PlayedYears.Add(gameState.CurrentYear);
                context.SaveChanges();
            }
            ContractService.SponsorPayments();
            //UpdateTeamRankingsInitially();
        }

        public static void UpdateTeamRankingsInitially()
        {
            var allTeams = context.Teams.Where(t => t.GameStateId == gameState.Id).ToList();
            //var allMatches = GetAllCompletedMatches("CS");

            // Calculate team scores with the new algorithm
            foreach (var team in allTeams)
            {
                DefineTeamRegion(team);
                UpdateVRS(team, 0);
            }

            var teamScores = allTeams.Select(team =>
            {
                double compositionScore = CalculateTeamCompositionScore(team);
                double strengthScore = CalculateTeamStrength(team);
                //double performanceScore = CalculateRecentPerformance(team, allMatches);

                return new
                {
                    Team = team,
                    TotalScore = compositionScore * strengthScore //* performanceScore
                };
            })
            .OrderByDescending(t => t.TotalScore)
            .ToList();

            // Assign rankings based on the new scores
            for (int i = 0; i < teamScores.Count(); i++)
            {
                teamScores[i].Team.WorldRanking = i + 1;
                //teamScores[i].Team.TeamScore = teamScores[i].TotalScore;
            }
            context.SaveChanges();
        }
        private static double CalculateTeamCompositionScore(Team team)
        {
            // Check for required roles
            var roles = team.Players.SelectMany(p => p.GameProfiles.OfType<GameProfile_CS>()).Select(c=>c.Role).ToList();

            bool hasRifler = roles.Contains(Role_CS.Rifler);
            bool hasAWPer = roles.Contains(Role_CS.AWPer);
            bool hasIGL = roles.Contains(Role_CS.InGameLeader);
            bool hasSupport = roles.Contains(Role_CS.Support);
            bool hasLurker = roles.Contains(Role_CS.Lurker);

            // Base composition score (1.0 = perfect composition)
            double compositionScore = 1.0;

            if (!hasRifler) compositionScore *= 0.85;
            if (!hasAWPer) compositionScore *= 0.80;
            if (!hasIGL) compositionScore *= 0.70;
            if (!hasSupport) compositionScore *= 0.90;
            if (!hasLurker) compositionScore *= 0.95;

            // Bonus for having all roles
            if (hasRifler && hasAWPer && hasIGL && hasSupport && hasLurker)
                compositionScore *= 1.10;

            return compositionScore;
        }

        private static double CalculateTeamStrength(Team team)
        {
            if (team == null || team.Players.Count == 0) return 0;

            double totalStrength = 0;
            double roleSynergyBonus = 1.0;
            foreach (var player in team.Players)
            {
                double playerStrength = player.Rating;
                var profile = context.GameProfiles.OfType<GameProfile_CS>()
                    .FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);

                switch (profile.Role)
                {
                    case Role_CS.InGameLeader:
                        playerStrength *= 1 + (profile.GameSense * 0.05 + profile.Communication*0.01);
                        break;
                    case Role_CS.AWPer:
                        playerStrength *= 1 + (profile.Mechanics * 0.03 + profile.Reflexes * 0.01);
                        break;
                    case Role_CS.Support:
                        playerStrength *= 1 + (profile.Teamwork * 0.05 + profile.Adaptability*0.01);
                        break;
                    case Role_CS.Lurker:
                        playerStrength *= 1 + (profile.Consistency * 0.04 + profile.GameSense*0.009);
                        break;
                    case Role_CS.Rifler:
                        playerStrength *= 1 + (profile.Mechanics * 0.09 + profile.Reflexes*0.3);
                        break;
                }

                totalStrength += playerStrength;
            }

            double averageStrength = totalStrength / team.Players.Count;
            return averageStrength * roleSynergyBonus;
        }
        private static double CalculateRecentPerformance(Team team, List<Match_CS> allMatches)
        {
            var teamMatches = allMatches
                .Where(m => m.Series.TeamA == team || m.Series.TeamB == team)
                .OrderByDescending(m => m.Series.Schedule)
                .Take(10)
                .ToList();

            if (!teamMatches.Any()) return 1.0;

            int wins = 0;
            int matches = 0;
            double totalPerformance = 0;

            foreach (var match in teamMatches)
            {
                bool isTeam1 = match.Series.TeamA == team;
                bool won = isTeam1 ? match.TeamAScore > match.TeamBScore : match.TeamBScore > match.TeamAScore;

                if (won) wins++;
                matches++;

                double performance = isTeam1 ?
                    (double)match.TeamAScore / (match.TeamAScore + match.TeamBScore) :
                    (double)match.TeamBScore / (match.TeamAScore + match.TeamBScore);

                totalPerformance += performance;
            }

            double winRate = (double)wins / matches;
            double avgPerformance = totalPerformance / matches;

            return (winRate * 0.6) + (avgPerformance * 0.4);
        }

        public static List<Match_CS> GetAllCompletedMatches(string game)
        {
            var g = context.Games.Where(c=>c.GameStateId==gameState.Id).FirstOrDefault(c=>c.ShortName==game);
            return context.Tournaments
                .Where(t=>t.GameId==g.Id)
                .SelectMany(t => t.Series)
                .Where(m => m.Status==SeriesStatus.Finished)
                .SelectMany(m=>m.Matches).OfType<Match_CS>()
                .ToList();
        }

        private static void DefineTeamRegion(Team team)
        {
            int _eu = 0;
            int _am = 0;
            int _as = 0;
            foreach (var player in team.Players)
            {
                if (_EU.Contains(player.Nationality))
                    _eu++;
                else if (_AM.Contains(player.Nationality))
                    _am++;
                else if (_AS.Contains(player.Nationality))
                    _as++;
            }
            team.Region = _as >= _eu && _as >= _am ? "AS" :
                     _am >= _eu ? "AM" : "EU";
        }

        public static void AwardVRSPoints(Team winner, Team loser, TournamentTier tier, TournamentType type)
        {
            double basePoints = VRSPoints.GetBasePoints(tier, type);
            double strengthFactor = CalculateTeamStrength(loser) / CalculateTeamStrength(winner);

            double pointsEarned = basePoints * strengthFactor;

            UpdateVRS(winner, pointsEarned);
            UpdateVRS(loser, -pointsEarned * 0.25); // loser loses some points
        }

        public static void UpdateVRS(Team team, double points)
        {
            var ranking = context.VRSRankings.FirstOrDefault(r => r.TeamId == team.Id);
            if (ranking == null)
            {
                ranking = new VRSRanking
                {
                    TeamId = team.Id,
                    Points = 0,
                    LastUpdated = DateTime.Now
                };
                context.VRSRankings.Add(ranking);
            }

            ranking.Points = Math.Max(0, ranking.Points + points);
            ranking.LastUpdated = DateTime.Now;

            context.SaveChanges();
        }
        public static void DecayVRSPoints()
        {
            var decayRate = 0.05; // 5% weekly decay
            var allRankings = context.VRSRankings.ToList();

            foreach (var ranking in allRankings)
            {
                ranking.Points *= 1 - decayRate;
                ranking.LastUpdated = DateTime.Now;
            }

            context.SaveChanges();
        }
        public static List<Team> GetVRSLeaderboard()
        {
            return context.VRSRankings
                .OrderByDescending(r => r.Points)
                .Select(r => r.Team)
                .ToList();
        }
    }
}
