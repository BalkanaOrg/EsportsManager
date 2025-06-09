using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class ContractSponsor : Contract
    {
        public Guid SponsorId { get; set; }
        public Sponsorship Sponsor { get; set; }

        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public Guid? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        public double PeriodPayment { get; set; } = 0;
        public ContractPeriodType PeriodType { get; set; }
        public ContractSponsorType SponsorType { get; set; }

        public int SatisfactionSponsor { get; set; } = 100;
        public int SatisfactionTeam { get; set; } = 200;
    }
}
