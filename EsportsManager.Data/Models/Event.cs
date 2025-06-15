using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Event
    {
        public Guid Id { get; set; } = new Guid();

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }

        public int[] Date { get; set; } = new int[2];

        public EventType EventType { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid GameId { get; set; }
    }
}
