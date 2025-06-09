using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Match_CS : Match
    {
        public Guid MapId { get; set; }
        public Map Map { get; set; }

        public TimeSpan Duration { get; set; }

        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }

        public List<PlayerStats_CS> PlayerStats { get; set; } = new();
    }
}
