using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Manager : Person
    {
        public Role_Manager Role { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public ICollection<ManagerTrait> ManagerTraits { get; set; }
    }
}
