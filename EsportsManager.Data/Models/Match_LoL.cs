using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Match_LoL : Match
    {
        public TimeSpan Duration { get; set; }

        public int KillsTeamA { get; set; }
        public int DeathsTeamA { get; set; }
        public int AssistsTeamA { get; set; }
        public int TowersDestroyedTeamA { get; set; }
        public int DragonsSlayedTeamA { get; set; }
        public int BaronsSlayedTeamA { get; set; }

        public int KillsTeamB { get; set; }
        public int DeathsTeamB { get; set; }
        public int AssistsTeamB { get; set; }
        public int TowersDestroyedTeamB { get; set; }
        public int DragonsSlayedTeamB { get; set; }
        public int BaronsSlayedTeamB { get; set; }
    }
}
