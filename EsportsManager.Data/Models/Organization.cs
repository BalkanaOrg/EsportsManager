using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace EsportsManager.EsportsManager.Data.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //Nqkva snimka trea da ima
        public int FoundingYear { get; set; }
        public int Prestige { get; set; } = 100;

        public OrganizationType Type { get; set; } = OrganizationType.Team;
        public bool isUserControlled { get; set; } = false;

        public double Budget { get; set; } = 0;
        public double WeeklyExpense { get; set; } = 0;

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }
        public List<GameState> GameStateReferences { get; set; } = new();

        public string HQ_Country { get; set; }
        public string HQ_City { get; set; }

        public int WeeklyMotivation { get; set; } = -1;
        public int WeeklyHappiness { get; set; } = -1;
        public int WeeklyPrestiegeDecay { get; set; } = -1;

        public List<Manager> Managers { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
        public List<Tournament> Tournaments { get; set; } = new();
        public List<ContractSponsor> Sponsors { get; set; } = new();

        public ICollection<OrganizationTrait> OrganizationTraits { get; set; }
    }
}
