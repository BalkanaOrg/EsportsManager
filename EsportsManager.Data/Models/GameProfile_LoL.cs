using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class GameProfile_LoL : GameProfile
    {
        public Role_LoL Role { get; set; }
        public bool isBenched { get; set; }
        // Gameplay stuff
        public double Potential { get; set; }
        public double Tactics { get; set; }
        public double Mechanics { get; set; }
        public double Reflexes { get; set; }
        public double Communication { get; set; }
        public double Teamwork { get; set; }
        public double GameSense { get; set; }
        public double Adaptability { get; set; }
        public double Consistency { get; set; }

        //Stats
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int CS { get; set; }
        public int GamesPlayed { get; set; }
        public int MinutesPlayed { get; set; }
        public int WardsPlaced { get; set; }
        public int WardsCleared { get; set; }
        public int ObjectivesTaken { get; set; }
        [NotMapped]
        public double KDA => Deaths > 0 ? (Kills + Assists) / Deaths : 0;
        [NotMapped]
        public double CSPM => MinutesPlayed > 0 ? (double)CS / MinutesPlayed : 0;
    }
}
