using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public enum EventType
    {
        TeamCreation,
        TeamDisbandment,
        TeamSignedByOrganization,
        TeamSoldByOrganization,
        PlayerTransfer,
        PlayerBenching,
        PlayerPromotion,
        PlayerRetirement,
        CoachTransfer,
        CoachBenching,
        CoachPromotion,
        CoachRetirement,
        ContractExpiration,
        ContractExtension,
        TeamSignedSponsor,
        TeamSponsorshipEnded,
        TeamSponsorshipCancelled,
        MapPoolChange,
        TournamentConclusion,
        MajorAnnouncement,
        TIAnnouncement,
        WorldsAnnouncement,
    }
}
