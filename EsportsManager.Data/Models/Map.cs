using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Map
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public Guid GameId { get; set; }
        public Game Game { get; set; } = new();

        public bool isActive { get; set; } = true;

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; } = new();

        public List<Match_CS> Matches { get; set; } = new();
    }
}
