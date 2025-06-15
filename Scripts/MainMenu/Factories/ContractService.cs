using EsportsManager.EsportsManager.Data.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.Time;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class ContractService
    {
        static GameState gameState = GameService.TransferGameState();
        static GameDbContext context = GameService.LoadDataBaseInformation();
        static int[] date = [0,0];

        public static void SignPlayerContract(Player player, Team team, double Buyout, double salary, double benchedSalary, double signingBonus, bool canBeTraded, bool canBeBenched, bool mustHaveConsent, double prizePoolPercentage, int[] contractExp)
        {
            player.WeeklySalary = salary;
            team.WeeklyExpense += salary;
            var contract = new ContractPlayer
            {
                Id = new Guid(),
                GameState = gameState,
                Player = player,
                Team = team,
                Buyout = Buyout,
                WeeklySalary = salary,
                SigningBonus = signingBonus,
                canBeSold = canBeTraded,
                canBeBenched = canBeBenched,
                mustHaveConsent = mustHaveConsent,
                PrizePoolPercent = prizePoolPercentage,
                BenchedSalary = benchedSalary,
                ExpirationDate = contractExp,
                isActive = true,
            };
            context.Contracts.Add(contract);
            context.SaveChanges();
        }

        public static void SignSponsor(Team team, Sponsorship sponsor, int[] endDate, double payment, ContractPeriodType paymentType)
        {
            var contract = new ContractSponsor
            {
                Id = new Guid(),
                GameState = gameState,
                Team = team,
                Sponsor = sponsor,
                ExpirationDate = endDate,
                PeriodPayment = payment,
                PeriodType = paymentType,
                isActive = true,
            };
            context.Contracts.Add(contract);
            context.SaveChanges();
        }
        public static void SignSponsor(Organization org, Sponsorship sponsor, int[] endDate, double payment, ContractPeriodType paymentType)
        {
            var contract = new ContractSponsor
            {
                Id = new Guid(),
                GameState = gameState,
                Organization = org,
                Sponsor = sponsor,
                ExpirationDate = endDate,
                PeriodPayment = payment,
                PeriodType = paymentType,
                isActive = true,
            };
            context.Contracts.Add(contract);
            context.SaveChanges();
        }
        public static void SponsorPayments()
        {
            date = new int[] { gameState.CurrentYear, gameState.CurrentWeek };
            int currentWeek = gameState.CurrentWeek;
            var allSponsorContractPayments = context.Contracts.OfType<ContractSponsor>().Where(c => c.GameStateId == gameState.Id && c.isActive == true).ToList();
            var weeklyPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.Weekly).ToList();
            var monthlyPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.Monthly).ToList();
            var quarterlyPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.Quarterly).ToList();
            var halfyearlyPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.HalfYearly).ToList();
            var yearlyPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.Yearly).ToList();
            var endofcontractPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.EndOfContract).ToList();
            var activationsPayments = allSponsorContractPayments.Where(c => c.PeriodType == ContractPeriodType.Activations).ToList();

            foreach (var contract in weeklyPayments)
            {
                contract.Team.Budget += contract.PeriodPayment;
                GD.Print($"{date[0]}, {date[1]}");
                if (ContractCheck(contract.ExpirationDate)) contract.isActive = false;
            }
            if (currentWeek % 4 == 0)
            {
                foreach (var contract in monthlyPayments)
                {
                    contract.Team.Budget += contract.PeriodPayment;
                    if (ContractCheck(contract.ExpirationDate)) contract.isActive = false;
                }
            }
            if (currentWeek % 12 == 0)
            {
                foreach (var contract in quarterlyPayments)
                {
                    contract.Team.Budget += contract.PeriodPayment;
                    if (ContractCheck(contract.ExpirationDate)) contract.isActive = false;
                }
            }
            if (currentWeek % 52 == 0)
            {
                foreach (var contract in yearlyPayments)
                {
                    contract.Team.Budget += contract.PeriodPayment;
                    if (ContractCheck(contract.ExpirationDate)) contract.isActive = false;
                }
            }
            context.SaveChanges();
        }

        public static void ProposeBuyout(Team buyer, Player player, Team seller, double buyout, double newBuyout, double salary, double benchedSalary, double signingBonus, bool canBeTraded, bool canBeBenched, bool mustHaveConsent, int[] contractExp)
        {

        }

        public static void BuyPlayerFromTeam(Team buyer, Player player, Team seller, double buyout, double newBuyout, double salary, double benchedSalary, double signingBonus, bool canBeTraded, bool canBeBenched, bool mustHaveConsent, double prizePoolPercent, int[] contractExp)
        {
            var oldContract = context.Contracts.OfType<ContractPlayer>()
                .FirstOrDefault(c => c.GameStateId == gameState.Id && c.PlayerId == player.Id && c.TeamId == seller.Id && c.isActive == true);
            if(oldContract == null)
            {

            }
            else
            {
                oldContract.isActive = false;
            }

            player.Team = buyer;
            if(seller.Players.Contains(player))
            {
                seller.Players.Remove(player);
            }
            else
            {
                seller.Bench.Remove(player);
                player.isBenched = false;
            }
            if(buyer.Players.Count<5)
            {
                buyer.Players.Add(player);
                player.isBenched = false;
            }
            else
            {
                buyer.Bench.Add(player);
                player.isBenched = true;
            }
            seller.Budget += buyout;
            buyer.Budget -= buyout;
            context.SaveChanges();
            SignPlayerContract(player, buyer, newBuyout, salary, benchedSalary, signingBonus, canBeTraded, canBeBenched, mustHaveConsent,prizePoolPercent, contractExp);
        }
        public static bool ContractCheck(int[] exp)
        {
            return exp[0] < date[0] || (exp[0] == date[0] && exp[1] <= date[1]);
        }

        public static int[] ReturnDate(int[] exp, int _weeks)
        {
            date = new int[] { gameState.CurrentYear, gameState.CurrentWeek };
            int weeks = date[1];
            int years = date[0];

            if(weeks - _weeks < 1)
            {
                years -= 1;
                weeks = 52 + (weeks - _weeks);
            }
            else
            {
                weeks -= _weeks;
            }
            return [years,weeks];
        }
        public static bool IsTeamSingleNational(Player player)
        {
            var q = context.Teams.FirstOrDefault(t => t.GameStateId == gameState.Id && t.Players.Any(p=>p.Id==player.Id));
            var nationalities = q.Players.Select(p => p.Nationality).Distinct();
            return nationalities.Count() == 1;
        }
        public static bool IsTeamSingleNational(Team team)
        {
            if (team == null || team.Players == null || !team.Players.Any())
                return false;

            var nationalities = team.Players.Select(p => p.Nationality).Distinct();
            return nationalities.Count() == 1;
        }
    }
}
