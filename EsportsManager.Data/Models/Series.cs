using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Series
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = new();

        public Guid TeamAId { get; set; }
        public Team TeamA { get; set; }

        public Guid TeamBId { get; set; }
        public Team TeamB { get; set; }

        public int[] Schedule { get; set; }

        public SeriesFormat Format { get; set; }

        public int TeamAWins { get; set; } = 0;
        public int TeamBWins { get; set; } = 0;

        public SeriesStatus Status { get; set; }

        public List<Match> Matches { get; set; } = new();
        public Team WinnerTeam => TeamAWins > TeamBWins ? TeamA : TeamB;
    }
}
