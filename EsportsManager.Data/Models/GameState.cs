using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class GameState
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int CurrentYear { get; set; } = 2025;
        public int CurrentWeek { get; set; } = 1;

        [AllowNull]
        public Guid? UserTeamId { get; set; }

        [AllowNull]
        public Guid? UserOrgId { get; set; }

        public decimal Budget { get; set; } = 0;
        public decimal WeeklyExpense { get; set; } = 0;
        public List<int> PlayedYears { get; set; } = new();

        [NotMapped]
        public List<Tournament> ActiveTournaments { get; set; } = new();
        [NotMapped]
        public List<Tournament> UpcomingTournaments { get; set; } = new();
        [NotMapped]
        public List<Tournament> CompletedTournaments { get; set; } = new();

        public List<Organization> Organizations { get; set; } = new();
        public List<Tournament> AllTournaments { get; set; } = new();
        public List<Game> AllGames { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
        public List<Player> Players { get; set; } = new();
        public List<Coach> Coaches { get; set; } = new();
        public List<Manager> Managers { get; set; } = new();
        public List<Map> Maps { get; set; } = new();
        public List<Sponsorship> Sponsors {  get; set; } = new();

        [NotMapped]
        public List<Map> MapPool => Maps.Where(m => m.isActive).DefaultIfEmpty().ToList();
    }
}
