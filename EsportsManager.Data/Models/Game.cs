using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }

        public Guid DeveloperId { get; set; }
        public Organization Developer { get; set; }

        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string IconURL { get; set; }
        public GameGenre Genre { get; set; }
        public string Description { get; set; }

        public int DedicatedPlayers { get; set; } = 0;

        public List<Team> Teams { get; set; } = new();
        public List<GameProfile> GameProfiles { get; set; } = new();
    }
}
