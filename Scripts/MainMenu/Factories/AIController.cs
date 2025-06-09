using EsportsManager.EsportsManager.Data.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class AIController
    {
        private static GameDbContext context = GameService.LoadDataBaseInformation();
        private static GameState gameState = GameService.TransferGameState();

        public static void MakeDecisions(Organization org)
        {

        }
        public static void MakeDecisions_CS(Team team)
        {
            if (ShouldSignPlayer(team)) FindPlayersToSign(team);
            if (ShouldSignCoach(team)) FindCoachesToSign(team);
            if (ShouldUnbenchPlayer(team)) return;
        }

        private static bool ShouldSignPlayer(Team team)
        {
            if(team.Budget < team.WeeklyExpense*32)
            {
                return false;
            }
            else
            {
                if(team.Players.Count<5 && team.Bench.Count<1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool ShouldUnbenchPlayer(Team team)
        {
            if (team.Bench.Count > 0 && team.Players.Count > 0)
            {
                bool isBenchPlayerBetterThanMain = false;
                foreach (var player in team.Bench)
                {
                    foreach(var p in team.Players)
                    {
                        isBenchPlayerBetterThanMain = ComparePlayers(player, p);
                    }
                }
                return isBenchPlayerBetterThanMain;
            }
            else if (team.Players.Count < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ShouldSignCoach(Team team)
        {
            if (team.Budget < team.WeeklyExpense * 30)
            {
                return false;
            }
            else
            {
                if (team.Coaches.Count <= 3)
                {
                    return true;
                }
                return false;
            }
        }

        private static List<Player> FindPlayersToSign(Team team)
        {
            var allPlayers = context.Players.Where(c => c.GameStateId == team.GameStateId).ToList();
            var freeAgents = allPlayers.Where(c => c.TeamId == null).ToList();

            var signedPlayers = allPlayers.Where(p => p.TeamId == team.Id).ToList();
            var signedRoles = new HashSet<Role_CS>();

            foreach (var player in signedPlayers)
            {
                var profile = context.GameProfiles.OfType<GameProfile_CS>()
                    .FirstOrDefault(gp => gp.PlayerId == player.Id && player.GameStateId == team.GameStateId);
                if (profile != null)
                    signedRoles.Add(profile.Role);
            }

            var compatiblePlayers = new List<Player>();

            foreach (var agent in freeAgents)
            {
                var profile = context.GameProfiles.OfType<GameProfile_CS>()
                    .FirstOrDefault(gp => gp.PlayerId == agent.Id && agent.GameStateId == team.GameStateId);
                if (profile == null)
                    continue;

                // Skip if role already taken
                if (signedRoles.Contains(profile.Role))
                    continue;

                // Mock a simple offer for testing (adjust based on your economy)
                double offeredSalary = agent.ExpectedSalary * 1.1;
                int[] contractDuration = { gameState.CurrentYear + 1, gameState.CurrentWeek };

                if (ShouldPlayerAcceptContract(agent, team, offeredSalary, contractDuration))
                {
                    compatiblePlayers.Add(agent);
                    //GD.Print($"Suitable Free Agent Found: {agent.FullName} - Role: {profile.Role}");
                }
            }

            return compatiblePlayers;
        }

        private static void FindCoachesToSign(Team team)
        {

        }
        private static bool ComparePlayers(Player p1, Player p2)
        {
            var profile1 = context.GameProfiles.OfType<GameProfile_CS>().FirstOrDefault(c => c.PlayerId == p1.Id);
            var profile2 = context.GameProfiles.OfType<GameProfile_CS>().FirstOrDefault(c => c.PlayerId == p2.Id);
            double score1=1, score2=1;
            switch (profile1.Role)
            {
                case Role_CS.Rifler:
                    score1 *= profile1.Mechanics * profile1.Reflexes * profile1.Consistency;
                break;
                case Role_CS.AWPer:
                    score1 *= profile1.Mechanics * profile1.Reflexes * profile1.Adaptability;
                break;
                case Role_CS.InGameLeader:
                    score1 *= profile1.GameSense * profile1.Tactics * profile1.Teamwork;
                break;
                case Role_CS.Lurker:
                    score1 *= profile1.Tactics * profile1.Adaptability * profile1.Consistency;
                break;
                case Role_CS.Support:
                    score1 *= profile1.Teamwork * profile1.Communication * profile1.Adaptability;
                break;
            }
            switch (profile2.Role)
            {
                case Role_CS.Rifler:
                    score2 *= profile2.Mechanics * profile2.Reflexes * profile2.Consistency;
                break;
                case Role_CS.AWPer:
                    score2 *= profile2.Mechanics * profile2.Reflexes * profile2.Adaptability;
                break;
                case Role_CS.InGameLeader:
                    score2 *= profile2.GameSense * profile2.Tactics * profile2.Teamwork;
                break;
                case Role_CS.Lurker:
                    score2 *= profile2.Tactics * profile2.Adaptability * profile2.Consistency;
                break;
                case Role_CS.Support:
                    score2 *= profile2.Teamwork * profile2.Communication * profile2.Adaptability;
                break;
            }
            if (score1 >= score2) return true;
            else return false;
        }

        private static bool ShouldPlayerAcceptContract(Player player, Team team, double salary, int[] contractDuration)
        {
            if (salary < player.ExpectedSalary)
                return false;

            int endYear = contractDuration[0];
            int endWeek = contractDuration[1];
            int currentYear = gameState.CurrentYear;
            int currentWeek = gameState.CurrentWeek;

            int totalWeeks = (endYear - currentYear) * 52 + (endWeek - currentWeek);

            // Reject too-long contracts
            if (totalWeeks > 104 && player.Age < 26) // >2 years
                return false;

            // High-rating players want top 10 teams
            if (player.Rating > 75)
            {
                if (player.Team == null)
                {
                    if (team.WorldRanking > 10)
                        return false;
                }
                else
                {
                    if (team.WorldRanking > 10 && player.Team.WorldRanking <= 20)
                        return false;
                }
            }

            return true;
        }
    }
}
