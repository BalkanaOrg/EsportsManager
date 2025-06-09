using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public abstract class GameProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
