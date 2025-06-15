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

                if (ShouldPlayerAcceptContract(team, agent, 0, offeredSalary, 20, 0,0, true, true, true, contractDuration))
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

        public static bool ShouldPlayerAcceptContract(Team team, Player player, double buyout, double salary, double benchedSalary, double signingBonus, double prizePool, bool canBeTraded, bool canBeBenched, bool mustHaveConsent, int[] contractDuration)
        {
            double acceptance = 0;
            double baseAcceptance = (player.Rating * 1.2 * player.Age * 0.1);
            int endYear = contractDuration[0];
            int endWeek = contractDuration[1];
            int currentYear = gameState.CurrentYear;
            int currentWeek = gameState.CurrentWeek;

            int totalWeeks = (endYear - currentYear) * 52 + (endWeek - currentWeek);

            //Is salary enough
            if (salary >= player.ExpectedSalary)
            {
                acceptance += (salary/player.ExpectedSalary)*10;
            }
            //Is buyout too high
            if(buyout > player.ExpectedSalary*20)
            {
                acceptance -= (buyout/player.ExpectedSalary);
            }
            else
            {
                acceptance += 10;
            }
            //Signing bonus to sweeten the deal
            if(signingBonus>0)
            {
                acceptance += signingBonus / 150;
            }

            if (ContractService.IsTeamSingleNational(team)) acceptance += 20;

            // Reject too-long contracts
            if (totalWeeks > 104 && player.Age < 26) // >2 years
                acceptance-=30;

            // High-rating players want top 10 teams
            if (player.Rating > 75)
            {
                if (player.Team == null)
                {
                    if (team.WorldRanking > 10)
                        acceptance-=30;
                }
                else
                {
                    if (team.WorldRanking > 10 && (player.Team.WorldRanking != null && player.Team.WorldRanking <= 20 && player.Team.WorldRanking!=0))
                        acceptance -= 30;
                }
            }

            if(prizePool > 0)
            {
                acceptance += prizePool;
            }
            else
            {
                acceptance -= 10;
            }

            if (mustHaveConsent) acceptance += 20;
            if (canBeTraded) acceptance += 20;
            if (canBeBenched) acceptance -= 20;

            acceptance += team.Prestige / 10;

            GD.Print("Acceptance: " + acceptance.ToString());
            GD.Print("Base acceptance: " + baseAcceptance.ToString());
            if (acceptance > baseAcceptance) return true;
            else return false;
        }
        public static bool ShouldTeamAcceptBuyout(Team team, Player player, Team buyer, double buyout, bool doesPlayerWantToSign)
        {
            double acceptance = 0;
            var teamPlayers = context.Players.Where(p => p.TeamId == team.Id && p.GameStateId == team.GameStateId).OrderByDescending(c => c.Rating).ToList();
            for (int i = 0; i < teamPlayers.Count; i++)
            {
                if (teamPlayers[i] == player)
                {
                    switch (i)
                    {
                        case 0:
                            acceptance -= 50;
                            break;
                        case 1:
                            acceptance -= 30;
                            break;
                        case 2:
                            acceptance -= 10;
                            break;
                    }
                }
            }

            if (buyout >= player.Buyout)
            {
                double buyoutAboveExpectation = (buyout - player.Buyout) / 100;
                acceptance += buyoutAboveExpectation;
            }

            int[] date = ContractService.ReturnDate([gameState.CurrentYear, gameState.CurrentWeek], 26);
            var result = context.Series
                .Where(s =>
                    ((s.TeamAId == team.Id && s.TeamBId == buyer.Id) ||
                    (s.TeamAId == buyer.Id && s.TeamBId == team.Id)) && (s.Schedule[0] >= date[0] && s.Schedule[1] >= date[1])).DefaultIfEmpty()
                .ToList();
            if(result.Count >=0 && result.Count < 2)
            {
                acceptance += 20;
            }
            else if(result.Count>=10)
            {
                acceptance -= 30;
            }
            else
            {
                acceptance -= 10;
            }

            if (team.WorldRanking + 10 > buyer.WorldRanking)
            {
                acceptance += 10;
            }
            else if (team.WorldRanking < buyer.WorldRanking)
            {
                acceptance -= 10;
            }


            if (doesPlayerWantToSign) acceptance += 20;
            GD.Print("Team Acceptance: " + acceptance.ToString());
            if (acceptance > 50) return true;
            else return false;
        }
    }
}
