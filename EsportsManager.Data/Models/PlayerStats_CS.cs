using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class PlayerStats_CS : PlayerStats
    {
        public Guid MatchId { get; set; }
        public Match_CS Match { get; set; } = new();

        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }

        public int DamageDealt { get; set; }
        public int HeadShotKills { get; set; }

        public int RoundsPlayed { get; set; }
        public int RoundsWithKillsOrAssists { get; set; }
        public int RoundsTraded { get; set; }
        public int RoundsSurvived { get; set; }


        [NotMapped]
        public int HeadShotPercentage => Kills > 0 ? (HeadShotKills / Kills) * 100 : 0;
        [NotMapped]
        public int KAST => RoundsPlayed > 0 ?
            (RoundsWithKillsOrAssists + RoundsTraded + RoundsSurvived) / RoundsPlayed * 100 : 0;
        [NotMapped]
        public double KPR => RoundsPlayed > 0 ? (double)Kills / RoundsPlayed : 0;
        [NotMapped]
        public double DPR => RoundsPlayed > 0 ? (double)Deaths / RoundsPlayed : 0;
        [NotMapped]
        public double ADR => RoundsPlayed > 0 ? (double)DamageDealt / RoundsPlayed : 0;
        public double Impact => 2.13 * KPR + 0.42 * (Assists / (RoundsPlayed > 0 ? (double)RoundsPlayed : 1)) - 0.41;
        public double Rating => 0.0073 * KAST + 0.3591 * KPR - 0.5329 * DPR + 0.2372 * Impact + 0.032 * ADR + 0.1587;
    }
}
