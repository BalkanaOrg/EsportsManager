using EsportsManager.EsportsManager.Data.Models;
using Godot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
	public static class TournamentService
	{
		private static GameDbContext context = GameService.LoadDataBaseInformation();
		private static GameState gameState = GameService.TransferGameState();
		private static Random random = new Random();

        public static void CreateSeries(Tournament tournament, Team team1, Team team2, int bestOf, string game, int[] schedule)
		{
			//if (tournament.Series.Any(s => s.TeamAId == team1.Id && s.TeamBId == team2.Id))
			//{
			//    throw new InvalidOperationException("This series already exists in the tournament.");
			//}
			Series newSeries = new Series
			{
				Tournament = tournament,
				TeamA = team1,
				TeamB = team2,
				Format = SeriesFormat.Bo3,
				Status = SeriesStatus.Pending
			};
			context.Series.Add(newSeries);
			context.SaveChanges();
			SimulateMapVeto(team1, team2, bestOf, newSeries, game);
			context.SaveChanges();
		}

		public static void PlaySeries_CS(Series series)
		{
			var trackedSeries = context.Series
				.Where(s => s.Id == series.Id)
				.Include(s => s.Matches)
				.FirstOrDefault();

			if (trackedSeries == null)
			{
				GD.Print("Series not found in the database.");
				return;
			}

			var csMatches = trackedSeries.Matches.OfType<Match_CS>().OrderBy(m => m.MatchNumber).ToList();
			GD.Print($"Begins playing series: #{trackedSeries.TeamA.WorldRanking}|{trackedSeries.TeamA.Name} vs #{trackedSeries.TeamB.WorldRanking}|{trackedSeries.TeamB.Name}");
			if (csMatches.Count == 0)
			{
				GD.Print("No matches to play in this series.");
				return;
			}
			int teamAWins = 0;
			int teamBWins = 0;
			switch (series.Format)
			{
				case SeriesFormat.Bo1:
					var match = csMatches.First();
					PlayMatch_CS(match);
					GD.Print(match.Map.Name);
					if (match.WinnerTeamId == trackedSeries.TeamAId)
						teamAWins = 1;
					else
						teamBWins = 1;
					break;

				case SeriesFormat.Bo3:
					foreach (var bo3Match in csMatches)
					{
						GD.Print(bo3Match.Map.Name);
						if (teamAWins < 2 && teamBWins < 2)
						{
							PlayMatch_CS(bo3Match);

							if (bo3Match.WinnerTeamId == trackedSeries.TeamAId) { teamAWins++; GD.Print($"{trackedSeries.TeamA.Name} wins 1 map"); }
							else { teamBWins++; GD.Print($"{trackedSeries.TeamB.Name} wins 1 map"); }
						}
					}
					break;

				case SeriesFormat.Bo5:
					foreach (var bo5Match in csMatches)
					{
						if (teamAWins < 3 && teamBWins < 3)
						{
							PlayMatch_CS(bo5Match);

							if (bo5Match.WinnerTeamId == trackedSeries.TeamAId)
								teamAWins++;
							else
								teamBWins++;
						}
					}
					break;
				case SeriesFormat.Bo7:
					foreach (var bo7Match in csMatches)
					{
						if (teamAWins < 4 && teamBWins < 4)
						{
							PlayMatch_CS(bo7Match);

							if (bo7Match.WinnerTeamId == trackedSeries.TeamAId)
								teamAWins++;
							else
								teamBWins++;
						}
					}
					break;
					// Add more formats as needed
			}

			series.TeamAWins = teamAWins;
			series.TeamBWins = teamBWins;
			Team winnerTeam = series.TeamAWins>series.TeamBWins ? series.TeamA : series.TeamB;
			Team loserTeam = series.TeamAWins>series.TeamBWins ? series.TeamB : series.TeamA;
			GameService.AwardVRSPoints(winnerTeam, loserTeam, series.Tournament.Tier, series.Tournament.Type);
			trackedSeries.Status = SeriesStatus.Finished;

			context.SaveChanges();
		}
		private static void PlayMatch_CS(Match_CS match)
		{
			const int MaxRounds = 24;
			const int RoundsToWin = 13;
			const int MaxOvertimeRounds = 6;
			const int StartMoney = 800;
			const int RoundWinBonus = 3250;
			const int RoundLossBonusStart = 1400;
			const int RoundLossBonusIncrement = 500;
			const int MaxRoundLossBonus = 3400;

			var teamAPlayers = match.Series.TeamA.Players.ToList();
			var teamBPlayers = match.Series.TeamB.Players.ToList();

            var teamALoadouts = new Dictionary<Guid, List<WeaponType>>();
            var teamBLoadouts = new Dictionary<Guid, List<WeaponType>>();

            int teamARounds = 0;
			int teamBRounds = 0;

			// Store team economy
			var teamAMoney = teamAPlayers.ToDictionary(p => p.Id, _ => StartMoney);
			var teamBMoney = teamBPlayers.ToDictionary(p => p.Id, _ => StartMoney);
			int teamALossStreak = 0;
			int teamBLossStreak = 0;

			int totalRounds = 0;

			while ((teamARounds < RoundsToWin && teamBRounds < RoundsToWin) && totalRounds < MaxRounds)
			{
				totalRounds++;

				// Simulate round
				bool teamAWins = SimulateRound(
					match,
					teamAPlayers, teamBPlayers,
					teamAMoney, teamBMoney,
					match.Map,
					out List<Player> survivingA,
					out List<Player> survivingB,
					teamALoadouts,
					teamBLoadouts);

				if (teamAWins)
				{
					teamARounds++;
					teamALossStreak = 0;
					teamBLossStreak++;
					ApplyEconomy(teamAPlayers, teamAMoney, true, 0, teamALoadouts);
					ApplyEconomy(teamBPlayers, teamBMoney, false, teamBLossStreak, teamBLoadouts);
				}
				else
				{
					teamBRounds++;
					teamBLossStreak = 0;
					teamALossStreak++;
					ApplyEconomy(teamBPlayers, teamBMoney, true, 0, teamALoadouts);
					ApplyEconomy(teamAPlayers, teamAMoney, false, teamALossStreak, teamBLoadouts);
				}

				UpdateStats(match,teamAPlayers, teamBPlayers, survivingA, survivingB);
			}

			// Handle overtime
			if (teamARounds == teamBRounds)
			{
				int otA = 0;
				int otB = 0;
				for (int i = 0; i < MaxOvertimeRounds; i++)
				{
					bool teamAWins = SimulateRound(
						match,
						teamAPlayers, teamBPlayers,
						teamAMoney, teamBMoney,
						match.Map,
						out List<Player> survivingA,
						out List<Player> survivingB,
						teamALoadouts,
						teamBLoadouts);

					if (teamAWins) otA++;
					else otB++;

					UpdateStats(match,teamAPlayers, teamBPlayers, survivingA, survivingB);
				}

				if (otA > otB) teamARounds++;
				else teamBRounds++;
			}

			if (teamARounds > teamBRounds)
			{
				match.WinnerTeam = match.Series.TeamA;
			}
			else
			{
				match.WinnerTeam = match.Series.TeamB;
			}
			match.TeamAScore = teamARounds;
			match.TeamBScore = teamBRounds;
			foreach (var player in teamAPlayers.Concat(teamBPlayers))
			{
				var profile = context.GameProfiles.OfType<GameProfile_CS>()
					.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
				profile.MapsPlayed++;
            }
			context.SaveChanges();
		}
		private static double[] MapFactor(Map map)
		{
			double MapFirepowerFactor = 1;
			double MapTacticsFactor = 1;
			double MapUtilityFactor = 1;
			double MapBigGameFactor = 1;

			switch (map.Name)
			{
				case "de_dust2":
					MapFirepowerFactor = 1.5;
					MapTacticsFactor = 0.9;
					MapUtilityFactor = 0.9;
					MapBigGameFactor = 1.3;
					break;
				case "de_inferno":
					MapFirepowerFactor = 1.1;
					MapTacticsFactor = 1.3;
					MapUtilityFactor = 1.5;
					MapBigGameFactor = 1.4;
					break;
				case "de_nuke":
					MapFirepowerFactor = 1.4;
					MapTacticsFactor = 1.7;
					MapUtilityFactor = 1.3;
					MapBigGameFactor = 1.5;
					break;
				case "de_mirage":
					MapFirepowerFactor = 1.8;
					MapTacticsFactor = 1.3;
					MapUtilityFactor = 1.4;
					MapBigGameFactor = 1.8;
					break;
				default:
					MapFirepowerFactor = 1;
					MapTacticsFactor = 1;
					MapUtilityFactor = 1;
					MapBigGameFactor = 1;
					break;
			}
			return new double[]
			{
				MapFirepowerFactor,
				MapTacticsFactor,
				MapUtilityFactor,
				MapBigGameFactor
			};
		}

		private static bool SimulateRound(Match_CS match,List<Player> teamA, List<Player> teamB,
								  Dictionary<Guid, int> moneyA, Dictionary<Guid, int> moneyB,
								  Map map,
								  out List<Player> survivingA,
								  out List<Player> survivingB,
                                  Dictionary<Guid, List<WeaponType>> TeamAloadouts,
                                  Dictionary<Guid, List<WeaponType>> TeamBloadouts)
		{
            //double teamAScore = CalculateTeamScore(teamA, moneyA, map, TeamAloadouts);
            //double teamBScore = CalculateTeamScore(teamB, moneyB, map, TeamBloadouts);
            //teamAScore += RandomBias();
            //teamBScore += RandomBias();
            //GD.Print($"TeamA: {teamAScore} vs TeamB: {teamBScore}");

            //bool teamAWins = teamAScore > teamBScore;

            //survivingA = teamAWins ? teamA.OrderBy(_ => Guid.NewGuid()).Take(3).ToList() : new List<Player>();
            //survivingB = !teamAWins ? teamB.OrderBy(_ => Guid.NewGuid()).Take(3).ToList() : new List<Player>();

            var strengthA = GetCombatants(teamA, moneyA, map, TeamAloadouts);
            var strengthB = GetCombatants(teamB, moneyB, map, TeamBloadouts);

            survivingA = new List<Player>(teamA);
            survivingB = new List<Player>(teamB);

            int maxSkirmishes = 20;
            var killFeed = new List<(Player killer, Player victim, Player? assist)>();
            for (int i = 0; i < maxSkirmishes; i++)
            {
                if (survivingA.Count == 0 || survivingB.Count == 0)
                    break;

                // Randomly choose group sizes for each side (1 to 3 players)
                int groupASize = Math.Min(random.Next(1, 4), survivingA.Count);
                int groupBSize = Math.Min(random.Next(1, 4), survivingB.Count);

                // Pick random players for each group
                var groupA = PickRandomPlayers(survivingA, groupASize, random);
                var groupB = PickRandomPlayers(survivingB, groupBSize, random);

                // Calculate combined strength of each group
                double groupAStrength = groupA.Sum(p => strengthA[p.Id]);
                double groupBStrength = groupB.Sum(p => strengthB[p.Id]);

                // Add some randomness
                groupAStrength *= 1 + (random.NextDouble() - 0.5) * 0.2; // ±10%
                groupBStrength *= 1 + (random.NextDouble() - 0.5) * 0.2;

                // Determine winning side
                bool aWins = groupAStrength > groupBStrength;
				var winners = aWins ? groupA : groupB;

                // Calculate casualties on losing side
                var losers = aWins ? groupB : groupA;

                // Casualties: at least 1, up to half the losing group randomly
                int casualtiesCount = Math.Max(1, random.Next(1, losers.Count / 2 + 1));

                // Remove casualties from survivors
                for (int c = 0; c < casualtiesCount; c++)
                {
                    var victim = losers[random.Next(losers.Count)];
                    losers.Remove(victim);
                    if (aWins)
                        survivingB.Remove(victim);
                    else
                        survivingA.Remove(victim);

                    // Choose killer randomly from winners
                    var killer = winners[random.Next(winners.Count)];

                    // 30% chance of assist by someone else in the group
                    Player? assist = null;
                    if (winners.Count > 1 && random.NextDouble() < 0.3)
                        assist = winners.Where(p => p != killer).OrderBy(_ => random.Next()).First();

                    killFeed.Add((killer, victim, assist));
                }
            }

            foreach (var (killer, victim, assist) in killFeed)
            {
                var killerProfile = context.GameProfiles.OfType<GameProfile_CS>().Where(c => c.PlayerId == killer.Id && c.Player.GameStateId == gameState.Id).FirstOrDefault();
                var killerStats = context.PlayerStats.OfType<PlayerStats_CS>().Where(c => c.PlayerId == killer.Id && c.MatchId == match.Id).FirstOrDefault();

                killerProfile.Kills++;
                killerProfile.RoundsWithKillsOrAssists++;
                killerStats.Kills++;
                killerStats.RoundsWithKillsOrAssists++;

                var victimProfile = context.GameProfiles.OfType<GameProfile_CS>().Where(c => c.PlayerId == victim.Id && c.Player.GameStateId == gameState.Id).FirstOrDefault();
                var victimStats = context.PlayerStats.OfType<PlayerStats_CS>().Where(c => c.PlayerId == victim.Id && c.MatchId == match.Id).FirstOrDefault();

                victimProfile.Deaths++;
                victimStats.Deaths++;

                if (assist != null)
                {
                    var assistProfile = context.GameProfiles.OfType<GameProfile_CS>().Where(c => c.PlayerId == assist.Id && c.Player.GameStateId == gameState.Id).FirstOrDefault();
                    var assistStats = context.PlayerStats.OfType<PlayerStats_CS>().Where(c => c.PlayerId == assist.Id && c.MatchId == match.Id).FirstOrDefault();

                    assistProfile.Assists++;
                    assistProfile.RoundsWithKillsOrAssists++;
                    assistStats.Assists++;
                    assistStats.RoundsWithKillsOrAssists++;
                }

                if (killFeed.Any(k => k.victim == killer))
                {
                    victimStats.RoundsTraded++;
                    victimProfile.RoundsTraded++;
                }
            }


            bool teamAWins = survivingA.Count > 0;

            // Return surviving players of each team
            if (!teamAWins) survivingA.Clear();
            if (teamAWins) survivingB.Clear();

            return teamAWins;
		}

        private static int RandomBias()
        {
            // Biased around 0 with ~68% results within ±50
            double stddev = 50;
            double mean = 0;
            double u1 = 1.0 - random.NextDouble(); // [0,1)
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2); // Box-Muller
            double randNormal = mean + stddev * randStdNormal;
            return (int)Math.Clamp(randNormal, -149, 149);
        }

        private static Dictionary<Guid, double> GetCombatants(
			List<Player> team,
			Dictionary<Guid, int> money,
			Map map,
			Dictionary<Guid, List<WeaponType>> loadouts)
        {
            var dict = new Dictionary<Guid, double>();
            var factors = MapFactor(map);
            double FirepowerFactor = factors[0];
            double TacticsFactor = factors[1];
            double UtilityFactor = factors[2];
            double BigGameFactor = factors[3];

            foreach (var player in team)
            {
                var profile = context.GameProfiles.OfType<GameProfile_CS>()
                    .FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);

                double firepower = 1 + profile.Mechanics * 0.1 + profile.Reflexes * 0.1;
                double clutch = 1 + profile.Adaptability * 0.01 + profile.Consistency;
                double utility = 1 + profile.Tactics * 0.1 + profile.GameSense * 0.1;
                double teamWork = 1 + profile.Teamwork * 0.1 + profile.Communication * 0.1;

                // Get player's loadout from the dictionary
                loadouts.TryGetValue(player.Id, out var playerLoadout);

                double weaponBonus = 0;
                if (playerLoadout != null)
                {
                    foreach (var weapon in playerLoadout)
                    {
                        weaponBonus += WeaponValue(weapon);
                    }
                }
                weaponBonus /= 5000; // Normalize weapon value

                double ecoFactor = 0.9 + (money[player.Id] / 16000.0) * 0.2;

                double strength = (firepower * FirepowerFactor + clutch * BigGameFactor + utility + teamWork + weaponBonus) * ecoFactor;

                dict[player.Id] = strength;
            }
            return dict;
        }

        private static double WeaponValue(WeaponType weaponName)
        {
            switch (weaponName.ToString().ToLower())
            {
                case "awp": return 4750;
                case "ak47": return 2700;
                case "m4a1": return 3100;
                case "usp": return 200;
                case "p250": return 300;
                case "knife": return 0; // Knife always available
                case "grenade": return 300;
                default: return 1000; // Default average
            }
        }

        private static List<Player> PickRandomPlayers(List<Player> players, int count, Random rng)
        {
            return players.OrderBy(_ => rng.Next()).Take(count).ToList();
        }

        private static double CalculateTeamScore(List<Player> team, Dictionary<Guid, int> money, Map map, Dictionary<Guid, List<WeaponType>> loadouts)
		{
			double[] factors = MapFactor(map);
			double 
				FirepowerFactor = factors[0],
				TacticsFactor = factors[1],
				UtilityFactor = factors[2],
				BigGameFactor = factors[3];
			double score = 0;
			foreach (var player in team)
			{
				var profile = context.GameProfiles.OfType<GameProfile_CS>()
					.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
				double firepower = 1 + profile.Mechanics*0.1+profile.Reflexes*0.1;
				double clutch = 1 + profile.Adaptability * 0.01 + profile.Consistency;
				double utility = 1 + profile.Tactics * 0.1 + profile.GameSense * 0.1;
				double teamWork = 1 + profile.Teamwork * 0.1 + profile.Communication * 0.1;

                double weaponMultiplier = 1.0;
                if (loadouts.TryGetValue(player.Id, out var weapons))
                {
                    foreach (var w in weapons)
                    {
                        weaponMultiplier += w switch
                        {
                            WeaponType.Rifle => 0.2,
                            WeaponType.SMG => 0.1,
                            WeaponType.Pistol => -0.05,
                            WeaponType.ArmorLight => 0.05,
                            WeaponType.ArmorHeavy => 0.08,
                            WeaponType.Utility => 0.05,
                            _ => 0
                        };
                    }
                }
                double ecoFactor = 0.9 + (money[player.Id] / 16000.0) * 0.2;

                score += (firepower * FirepowerFactor + clutch * BigGameFactor + utility + teamWork) * ecoFactor * weaponMultiplier;
			}
			return score;
		}

		private enum WeaponType
		{
            None,
            Pistol,
            SMG,
            Rifle,
            ArmorLight,
            ArmorHeavy,
            Utility
        }

        private static void ApplyEconomy(List<Player> team, Dictionary<Guid, int> money, bool won, int lossStreak, Dictionary<Guid, List<WeaponType>> loadouts)
        {
            int bonus = won ? 3250 : Math.Min(1400 + lossStreak * 500, 3400);

            foreach (var player in team)
            {
                // Add money
                money[player.Id] = Math.Min(money[player.Id] + bonus, 16000);

                // Decide what to buy based on money
                int currentMoney = money[player.Id];
                var loadout = new List<WeaponType>();

                if (currentMoney >= 5000)
                {
                    loadout.Add(WeaponType.Rifle);
                    loadout.Add(WeaponType.ArmorHeavy);
                    loadout.Add(WeaponType.Utility);
                    currentMoney -= 5000;
                }
                else if (currentMoney >= 3000)
                {
                    loadout.Add(WeaponType.SMG);
                    loadout.Add(WeaponType.ArmorLight);
                    currentMoney -= 3000;
                }
                else if (currentMoney >= 1500)
                {
                    loadout.Add(WeaponType.Pistol);
                    currentMoney -= 700;
                }
                else
                {
                    loadout.Add(WeaponType.Pistol); // save round
                }

                // Store remaining money
                money[player.Id] = currentMoney;

                // Save loadout
                loadouts[player.Id] = loadout;
            }
        }

        private static void UpdateStats(Match_CS match,List<Player> teamA, List<Player> teamB,
								List<Player> survivingA, List<Player> survivingB)
		{
			Random rng = new();

			foreach (var player in teamA.Concat(teamB))
			{
				var profile = context.GameProfiles.OfType<GameProfile_CS>()
					.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
				var matchProfile = context.PlayerStats.OfType<PlayerStats_CS>()
					.FirstOrDefault(ps => ps.PlayerId == player.Id && ps.Match.Id == match.Id);
				profile.RoundsPlayed++;
				matchProfile.RoundsPlayed++;
			}

			//foreach (var player in teamA)
			//{
			//	var profile = context.GameProfiles.OfType<GameProfile_CS>()
			//		.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
			//	var matchProfile = context.PlayerStats.OfType<PlayerStats_CS>()
			//		.FirstOrDefault(ps => ps.PlayerId == player.Id && ps.Match.Id == match.Id);
			//	if (rng.NextDouble() < 0.5) { profile.Kills++; profile.RoundsWithKillsOrAssists++; matchProfile.Kills++; matchProfile.RoundsWithKillsOrAssists++; }
			//	if (rng.NextDouble() < 0.3) { profile.Assists++; matchProfile.Assists++; profile.RoundsWithKillsOrAssists++; matchProfile.RoundsWithKillsOrAssists++; }
			//	if (!survivingA.Contains(player)) { profile.Deaths++; matchProfile.Deaths++; }
			//	else { profile.RoundsSurvived++; matchProfile.RoundsSurvived++; }
			//}

			//foreach (var player in teamB)
			//{
			//	var profile = context.GameProfiles.OfType<GameProfile_CS>()
			//		.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
			//	var matchProfile = context.PlayerStats.OfType<PlayerStats_CS>()
			//		.FirstOrDefault(ps => ps.PlayerId == player.Id && ps.Match.Id == match.Id);
			//	if (rng.NextDouble() < 0.5) { profile.Kills++; profile.RoundsWithKillsOrAssists++; matchProfile.Kills++; matchProfile.RoundsWithKillsOrAssists++; }
			//	if (rng.NextDouble() < 0.3) { profile.Assists++; matchProfile.Assists++; profile.RoundsWithKillsOrAssists++; matchProfile.RoundsWithKillsOrAssists++; }
			//	if (!survivingA.Contains(player)) { profile.Deaths++; matchProfile.Deaths++; }
			//	else { profile.RoundsSurvived++; matchProfile.RoundsSurvived++; }
			//}
		}

		private static List<string> DefineMapPool(string game)
		{
			var _game = context.Games.Where(c=>c.ShortName == game && c.GameStateId==gameState.Id).FirstOrDefault();
			return gameState.MapPool.Where(c => c.GameId == _game.Id).Select(c=>c.Name).ToList();
		}

		private static Map StringToMap(string mapName)
		{
			return context.Maps.FirstOrDefault(m => m.Name == mapName && m.GameStateId==gameState.Id);
		}

		public static void SimulateMapVeto(Team team1, Team team2, int bestOf, Series series, string game)
		{
			// Example map pool
			var mapPool = DefineMapPool(game);

			// Fetch map preferences based on match history (optional: handle nulls safely)
			var team1MapHistory = GetMapHistory(team1);
			var team2MapHistory = GetMapHistory(team2);

			// Start veto process
			List<string> vetoedMaps = new();
			List<string> pickedMaps = new();

			var rng = new Random();
			bool team1Starts = rng.Next(2) == 0;

			if (bestOf == 1)
			{
				// Ban until 1 map remains
				for (int i = 0; i < 6; i++)
				{
					string mapToBan;
					if ((i % 2 == 0) == team1Starts)
						mapToBan = BanMap(team1MapHistory, mapPool, vetoedMaps);
					else
						mapToBan = BanMap(team2MapHistory, mapPool, vetoedMaps);

					vetoedMaps.Add(mapToBan);
					mapPool.Remove(mapToBan);
					GD.Print($"{(i % 2 == 0 ? team1.Name : team2.Name)} bans {mapToBan}");
				}

				pickedMaps.Add(mapPool.First());
				GD.Print($"Final Map: {mapPool.First()}");
			}
			else if (bestOf == 3)
			{
				// Ban -> Ban -> Pick -> Pick -> Ban -> Ban -> Decider
				string ban1 = BanMap(team1MapHistory, mapPool, vetoedMaps);
				mapPool.Remove(ban1);
				GD.Print($"{team1.Name} bans {ban1}");

				string ban2 = BanMap(team2MapHistory, mapPool, vetoedMaps);
				mapPool.Remove(ban2);
				GD.Print($"{team2.Name} bans {ban2}");

				string pick1 = PickMap(team1MapHistory, mapPool, pickedMaps);
				mapPool.Remove(pick1);
				pickedMaps.Add(pick1);
				GD.Print($"{team1.Name} picks {pick1}");

				string pick2 = PickMap(team2MapHistory, mapPool, pickedMaps);
				mapPool.Remove(pick2);
				pickedMaps.Add(pick2);
				GD.Print($"{team2.Name} picks {pick2}");

				string ban3 = BanMap(team1MapHistory, mapPool, vetoedMaps);
				mapPool.Remove(ban3);
				GD.Print($"{team1.Name} bans {ban3}");

				string ban4 = BanMap(team2MapHistory, mapPool, vetoedMaps);
				mapPool.Remove(ban4);
				GD.Print($"{team2.Name} bans {ban4}");

				string decider = mapPool.First();
				pickedMaps.Add(decider);
				GD.Print($"Decider Map: {decider}");
			}
			else if (bestOf == 5)
			{
				// Ban -> Ban -> Remaining 5 maps are played
				string ban1 = BanMap(team1MapHistory, mapPool, vetoedMaps);
				mapPool.Remove(ban1);
				GD.Print($"{team1.Name} bans {ban1}");

				string ban2 = BanMap(team2MapHistory, mapPool, vetoedMaps);
				mapPool.Remove(ban2);
				GD.Print($"{team2.Name} bans {ban2}");

				string pick1 = PickMap(team1MapHistory, mapPool, pickedMaps);
				mapPool.Remove(pick1);
				pickedMaps.Add(pick1);
				GD.Print($"{team1.Name} picks {pick1}");

				string pick2 = PickMap(team2MapHistory, mapPool, pickedMaps);
				mapPool.Remove(pick2);
				pickedMaps.Add(pick2);
				GD.Print($"{team2.Name} picks {pick2}");

				string pick3 = PickMap(team1MapHistory, mapPool, pickedMaps);
				mapPool.Remove(pick3);
				pickedMaps.Add(pick3);
				GD.Print($"{team1.Name} picks {pick3}");

				string pick4 = PickMap(team2MapHistory, mapPool, pickedMaps);
				mapPool.Remove(pick4);
				pickedMaps.Add(pick4);
				GD.Print($"{team2.Name} picks {pick4}");

				string decider = mapPool.First();
				pickedMaps.Add(decider);
				GD.Print($"Decider Map: {decider}");

				//pickedMaps.AddRange(mapPool);
				//GD.Print($"Remaining Maps: {string.Join(", ", mapPool)}");
			}
			int j = 1; 
			var seriesWithTeams = context.Series
				.Include(s => s.TeamA).ThenInclude(t => t.Players)
				.Include(s => s.TeamB).ThenInclude(t => t.Players)
				.FirstOrDefault(s => s.Id == series.Id);
            foreach (string map in pickedMaps)
			{
				Match_CS match = new Match_CS
				{
					Series = series,
					Map = StringToMap(map),
					MatchNumber = j++,
					WinnerTeam = null,
				};
				context.Matches.Add(match);
                var players = seriesWithTeams.TeamA.Players
					.Concat(seriesWithTeams.TeamB.Players)
					.ToList();
                foreach (var player in players)
				{
                    var stats = new PlayerStats_CS
                    {
                        Player = player,
                        Team = player.Team,
                        Match = match,
                        Kills = 0,
                        Assists = 0,
                        Deaths = 0,
                        DamageDealt = 0,
                        HeadShotKills = 0,
                        RoundsPlayed = 0,
                        RoundsSurvived = 0,
                        RoundsTraded = 0,
                        RoundsWithKillsOrAssists = 0,
                    };
                    context.PlayerStats.Add(stats);
				}
				
			}
		}

		private static List<string> GetMapHistory(Team team)
		{
			return context.Series
				.Where(s => s.TeamAId == team.Id || s.TeamBId == team.Id)
				.Include(s => s.Matches) // Load the list of Matches in each Series
				.SelectMany(s => s.Matches) // Flatten to Matches
				.OfType<Match_CS>() // Only Match_CS has the Map
				.Where(m => m.Map != null) // Defensive check for nulls
				.GroupBy(m => m.Map.Name) // Group by map name
				.OrderByDescending(g => g.Count())
				.Select(g => g.Key) // Get map names
				.ToList();
		}

		private static string BanMap(List<string> history, List<string> pool, List<string> alreadyBanned)
		{
			// Try to ban the opponent's best map
			foreach (var map in history)
			{
				if (pool.Contains(map) && !alreadyBanned.Contains(map))
					return map;
			}

			// Fallback: random
			return pool[new Random().Next(pool.Count)];
		}

		private static string PickMap(List<string> history, List<string> pool, List<string> alreadyPicked)
		{
			// Try to pick preferred map
			foreach (var map in history)
			{
				if (pool.Contains(map) && !alreadyPicked.Contains(map))
					return map;
			}

			// Fallback: random
			return pool[new Random().Next(pool.Count)];
		}



	}
}
