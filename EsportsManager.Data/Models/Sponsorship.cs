using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Sponsorship
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public SponsorProduct Product { get; set; }
        public SponsorTier Tier { get; set; }
        // Navigation properties
        public List<ContractSponsor> SponsoredEntities { get; set; } = new();
    }
}
