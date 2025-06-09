using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Trait_Manager
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconURL { get; set; }

        public ICollection<ManagerTrait> ManagerTraits { get; set; }
    }
}
