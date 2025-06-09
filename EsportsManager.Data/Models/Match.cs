using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Match
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid SeriesId { get; set; }
        public Series Series { get; set; } = new();

        public int MatchNumber { get; set; }

        public Guid? WinnerTeamId { get; set; }
        public Team? WinnerTeam { get; set; } = new();
    }
}
