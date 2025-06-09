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
					teamAPlayers, teamBPlayers,
					teamAMoney, teamBMoney,
					match.Map,
					out List<Player> survivingA,
					out List<Player> survivingB);

				if (teamAWins)
				{
					teamARounds++;
					teamALossStreak = 0;
					teamBLossStreak++;
					ApplyEconomy(teamAPlayers, teamAMoney, true, 0);
					ApplyEconomy(teamBPlayers, teamBMoney, false, teamBLossStreak);
				}
				else
				{
					teamBRounds++;
					teamBLossStreak = 0;
					teamALossStreak++;
					ApplyEconomy(teamBPlayers, teamBMoney, true, 0);
					ApplyEconomy(teamAPlayers, teamAMoney, false, teamALossStreak);
				}

				UpdateStats(teamAPlayers, teamBPlayers, survivingA, survivingB);
			}

			// Handle overtime
			if (teamARounds == teamBRounds)
			{
				int otA = 0;
				int otB = 0;
				for (int i = 0; i < MaxOvertimeRounds; i++)
				{
					bool teamAWins = SimulateRound(
						teamAPlayers, teamBPlayers,
						teamAMoney, teamBMoney,
						match.Map,
						out List<Player> survivingA,
						out List<Player> survivingB);

					if (teamAWins) otA++;
					else otB++;

					UpdateStats(teamAPlayers, teamBPlayers, survivingA, survivingB);
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

		private static bool SimulateRound(List<Player> teamA, List<Player> teamB,
								  Dictionary<Guid, int> moneyA, Dictionary<Guid, int> moneyB,
								  Map map,
								  out List<Player> survivingA,
								  out List<Player> survivingB)
		{
			double teamAScore = CalculateTeamScore(teamA, moneyA, map);
			double teamBScore = CalculateTeamScore(teamB, moneyB, map);
			teamAScore += RandomBias();
			teamBScore += RandomBias();
			GD.Print($"TeamA: {teamAScore} vs TeamB: {teamBScore}");

			bool teamAWins = teamAScore > teamBScore;

			survivingA = teamAWins ? teamA.OrderBy(_ => Guid.NewGuid()).Take(3).ToList() : new List<Player>();
			survivingB = !teamAWins ? teamB.OrderBy(_ => Guid.NewGuid()).Take(3).ToList() : new List<Player>();

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

        private static double CalculateTeamScore(List<Player> team, Dictionary<Guid, int> money, Map map)
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
				double ecoFactor = 0.9 + (money[player.Id] / 16000.0)*0*2;

				score += (firepower * FirepowerFactor + clutch * BigGameFactor + utility + teamWork) * ecoFactor;
			}
			return score;
		}

		private static void ApplyEconomy(List<Player> team, Dictionary<Guid, int> money, bool won, int lossStreak)
		{
			int bonus = won ? 3250 : Math.Min(1400 + lossStreak * 500, 3400);
			foreach (var player in team)
			{
				money[player.Id] = Math.Min(money[player.Id] + bonus, 16000);
			}
		}

		private static void UpdateStats(List<Player> teamA, List<Player> teamB,
								List<Player> survivingA, List<Player> survivingB)
		{
			Random rng = new();

			foreach (var player in teamA.Concat(teamB))
			{
				var profile = context.GameProfiles.OfType<GameProfile_CS>()
					.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
				var matchProfile = context.PlayerStats.OfType<PlayerStats_CS>()
					.FirstOrDefault(ps => ps.PlayerId == player.Id && ps.Match.Series.Tournament.GameStateId == gameState.Id);
				profile.MapsPlayed++;
				profile.RoundsPlayed++;
				matchProfile.RoundsPlayed++;
			}

			foreach (var player in teamA)
			{
				var profile = context.GameProfiles.OfType<GameProfile_CS>()
					.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
				var matchProfile = context.PlayerStats.OfType<PlayerStats_CS>()
					.FirstOrDefault(ps => ps.PlayerId == player.Id && ps.Match.Series.Tournament.GameStateId == gameState.Id);
				if (rng.NextDouble() < 0.5) { profile.Kills++; profile.RoundsWithKillsOrAssists++; matchProfile.Kills++; matchProfile.RoundsWithKillsOrAssists++; }
				if (rng.NextDouble() < 0.3) { profile.Assists++; matchProfile.Assists++; profile.RoundsWithKillsOrAssists++; matchProfile.RoundsWithKillsOrAssists++; }
				if (!survivingA.Contains(player)) { profile.Deaths++; matchProfile.Deaths++; }
				else { profile.RoundsSurvived++; matchProfile.RoundsSurvived++; }
			}

			foreach (var player in teamB)
			{
				var profile = context.GameProfiles.OfType<GameProfile_CS>()
					.FirstOrDefault(p => p.PlayerId == player.Id && p.Player.GameStateId == gameState.Id);
				var matchProfile = context.PlayerStats.OfType<PlayerStats_CS>()
					.FirstOrDefault(ps => ps.PlayerId == player.Id && ps.Match.Series.Tournament.GameStateId == gameState.Id);
				if (rng.NextDouble() < 0.5) { profile.Kills++; profile.RoundsWithKillsOrAssists++; matchProfile.Kills++; matchProfile.RoundsWithKillsOrAssists++; }
				if (rng.NextDouble() < 0.3) { profile.Assists++; matchProfile.Assists++; profile.RoundsWithKillsOrAssists++; matchProfile.RoundsWithKillsOrAssists++; }
				if (!survivingA.Contains(player)) { profile.Deaths++; matchProfile.Deaths++; }
				else { profile.RoundsSurvived++; matchProfile.RoundsSurvived++; }
			}
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
