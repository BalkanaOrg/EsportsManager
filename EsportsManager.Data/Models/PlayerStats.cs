using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class PlayerStats
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = new();

        public Guid TeamId { get; set; }
        public Team Team { get; set; } = new();
    }
}
