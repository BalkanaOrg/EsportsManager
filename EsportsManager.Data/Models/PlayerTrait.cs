using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class PlayerTrait
    {
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public Guid TraitId { get; set; }
        public Trait_Player Trait { get; set; }

        // You can add additional properties to the join table if needed
        public DateTime AcquiredDate { get; set; }
    }
}
