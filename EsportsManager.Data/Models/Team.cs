using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }

        [Required]
        public string Name { get; set; }
        public string Tag { get; set; } 
        public int FoundingYear { get; set; }

        public double Budget { get; set; }
        public double WeeklyExpense { get; set; }

        public int Prestige { get; set; }
        public int DivisionalLevel { get; set; } = 1;
        public bool isUserControlled { get; set; } = false;

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        public Guid? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        // Navigation
        public List<Player> Players { get; set; } = new();
        public List<Player> Bench { get; set; } = new();
        public List<Coach> Coaches { get; set; } = new();
        public List<ContractSponsor> Sponsors { get; set; } = new();

        public ICollection<TeamTrait> TeamTraits { get; set; }

        public List<GameState> GameStateReferences { get; set; } = new();

        public string Region { get; set; }
        public string HQ_Country { get; set; }
        public string HQ_City { get; set; }

        public List<Tournament> TournamentsWon { get; set; } = new();
        public List<Tournament> TournamentsParticipated { get; set; } = new();
        [NotMapped]
        public bool isOrgless => OrganizationId == null;

        public int WorldRanking { get; set; }
        public int? VRSranking { get; set; } = 0;
        public double Momentum { get; set; } = 0;

    }
}
