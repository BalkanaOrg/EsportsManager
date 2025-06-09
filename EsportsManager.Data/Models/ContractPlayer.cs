using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class ContractPlayer : Contract
    {
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public double WeeklySalary { get; set; } = 0;
        public double SigningBonus { get; set; } = 0;
        public double Buyout { get; set; } = 0;
        public double BenchedSalary { get; set; } = 0;

        public bool canBeBenched { get; set; } = true;
        public bool canBeSold { get; set; } = true;
        public bool mustHaveConsent { get; set; } = false;
    }
}
