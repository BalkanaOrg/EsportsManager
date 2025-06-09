using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class TeamTrait
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public Guid TraitId { get; set; }
        public Trait_Team Trait { get; set; }

        // You can add additional properties to the join table if needed
        public DateTime AcquiredDate { get; set; }
    }
}
