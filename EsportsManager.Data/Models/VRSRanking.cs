using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class VRSRanking
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public double Points { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
