using EsportsManager.EsportsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public class PlayerService
    {
        GameDbContext context;
        GameState gameState;
        private Random random = new Random();

        public PlayerService(GameDbContext _context, GameState _gameState)
        {
            context = _context;
            gameState = _gameState;
        }

        public void SignFreeAgentPlayer(Player player, Team team)
        {
            if(player.WeeklySalary > team.Budget)
            {
                throw new InvalidOperationException("Team does not have enough budget to sign this player.");
            }
            else
            {
                if(team.Players.Count >= 5)
                {
                    team.Bench.Add(player);
                    team.WeeklyExpense += player.WeeklySalary;
                    player.isBenched = true;
                    player.TeamId = team.Id;
                }
                else 
                {
                    team.Players.Add(player);
                    team.WeeklyExpense += player.WeeklySalary;
                    player.TeamId = team.Id;
                }
            }
        }

        public void ReleasePlayer(Player player, Team team)
        {
            player.TeamId = null;
            team.WeeklyExpense -= player.WeeklySalary;
            team.Players.Remove(player);
            team.Bench.Remove(player);
            player.isBenched = false;
        }

        public void MoveToBench(Player player, Team team)
        {
            if (!team.Bench.Contains(player))
            {
                player.isBenched = true;
                team.Players.Remove(player);
                team.Bench.Add(player);
            }
        }

        public void MoveToActive(Player player, Team team)
        {
            if (team.Players.Count>=5)
            {
                throw new InvalidOperationException("Team already has 5 active players.");
            }
            else
            {
                if (team.Bench.Contains(player))
                {
                    player.isBenched = false;
                    team.Bench.Remove(player);
                    team.Players.Add(player);
                }
                else
                {
                    throw new InvalidOperationException("Player is not benched.");
                }
            }
        }

        public void HireCoach(Coach coach, Team team)
        {
            if (coach.WeeklySalary > team.Budget)
            {
                throw new InvalidOperationException("Team does not have enough budget to hire this coach.");
            }
            else
            {
                team.Coaches.Add(coach);
                team.WeeklyExpense += coach.WeeklySalary;
                coach.TeamId = team.Id;
            }
        }
        public void ReleaseCoach(Coach coach, Team team)
        {
            coach.TeamId = null;
            team.WeeklyExpense -= coach.WeeklySalary;
            team.Coaches.Remove(coach);
        }

        public void SetPlayerBuyout(Player player, double buyout)
        {
            if (player != null && buyout > 0)
            {
                player.Buyout = buyout;
            }
        }
        public void RemovePlayerBuyout(Player player)
        {
            if (player != null)
            {
                player.Buyout = 0;
            }
        }
        public void ExtendPlayerContract(Player player, int[] contract)
        {
            if (player != null && contract.Length == 2 && contract[0] > gameState.CurrentYear && contract[1] > gameState.CurrentWeek)
            {
                player.ContractExpiration = contract;
            }
            else
            {
                throw new ArgumentException("Invalid contract parameters.");
            }
        }
        public void IncreasePlayerStatsForYear(Player player)
        {
            switch(player.Team.Game.ShortName)
                {
                    case "CS":
                    var profile = context.GameProfiles.OfType<GameProfile_CS>().Where(p=>p.PlayerId == player.Id).FirstOrDefault();
                    if(player.Age < 27)
                    {
                        int maxValue = (int)profile.Potential % 4;
                        int minValue = (int)profile.Potential < 20 ? (int)profile.Potential % 6 : (int)profile.Potential % 7;
                        int r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        int rnd = random.Next(minValue, maxValue);
                        profile.Potential -= random.Next(minValue, maxValue);

                        //Mechanics
                        if (r == 0) profile.Mechanics += rnd;
                        else profile.Mechanics -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Tactics
                        if (r == 0) profile.Tactics += rnd;
                        else profile.Tactics -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Reflexes
                        if (r == 0) profile.Reflexes += rnd;
                        else profile.Reflexes -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Adaptability
                        if (r == 0) profile.Adaptability += rnd;
                        else profile.Adaptability -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Game sense
                        if (r == 0) profile.GameSense += rnd;
                        else profile.GameSense -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Teamwork
                        if (r == 0) profile.Teamwork += rnd;
                        else profile.Teamwork -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Communication
                        if (r == 0) profile.Communication += rnd;
                        else profile.Communication -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                        //Consistency
                        if (r == 0) profile.Consistency += rnd;
                        else profile.Consistency -= rnd;
                        r = (int)profile.Potential > 40 ? 0 : random.Next(0, 1);
                        rnd = random.Next(minValue, maxValue);
                    }
                    else
                    {
                        int randomPotential = random.Next(0, 15);
                        if (profile.Potential > randomPotential)
                        {
                            profile.Potential = randomPotential;
                        }
                        else profile.Potential--;
                    }

                        break;
                }
            context.SaveChanges();
        }
    }
}
