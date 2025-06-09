using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Player : Person
    {
        public int Prestiege { get; set; }
        public double Rating { get; set; }

        public double Buyout { get; set; } = 0;

        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public bool isRetired { get; set; } = false;
        public bool isBenched { get; set; } = false;

        //Gameplay stuff

        public List<GameProfile> GameProfiles { get; set; } = new();

        public List<PlayerStats_CS> PlayerStats_CS { get; set; } = new();

        public List<ContractPlayer> ContractPlayers { get; set; } = new();

        public ICollection<PlayerTrait> PlayerTraits { get; set; }
    }
}
