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
        static int[] date;

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
        public static bool ContractCheck(int[] exp)
        {
            return exp[0] < date[0] || (exp[0] == date[0] && exp[1] <= date[1]);
        }
    }
}
