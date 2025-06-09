using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class GameProfile_SC : GameProfile
    {
        public Role_SC Role { get; set; }
        public bool isBenched { get; set; }
        // Gameplay stuff
        public double Tactics { get; set; }
        public double Mechanics { get; set; }
        public double Micro { get; set; }
        public double Macro { get; set; }
        public double GameSense { get; set; }
        public double Adaptability { get; set; }

        //Stats
        public int APM { get; set; }
        public int MapsPlayed { get; set; }
    }
}
