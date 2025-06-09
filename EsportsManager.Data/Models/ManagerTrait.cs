using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class ManagerTrait
    {
        public Guid ManagerId { get; set; }
        public Manager Manager { get; set; }

        public Guid TraitId { get; set; }
        public Trait_Manager Trait { get; set; }

        // You can add additional properties to the join table if needed
        public DateTime AcquiredDate { get; set; }
    }
}
