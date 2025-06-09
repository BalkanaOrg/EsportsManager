using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Coach : Person
    {
        public int Prestiege { get; set; }
        public Role_Coach Role { get; set; }

        public double Buyout { get; set; } = 0;

        //Gameplay stuff
        public double Communication { get; set; }
        public double Strategy { get; set; }
        public double GameSense { get; set; }
        public double Adaptability { get; set; }
        public double Mentorship { get; set; }

        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

    }
}
