using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Contract
    {
        public Guid Id { get; set; }

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }

        public bool isActive { get; set; } = false;

        public int[] ExpirationDate { get; set; } // [Year, Month]
    }
}
