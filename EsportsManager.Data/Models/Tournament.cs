using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public class Tournament
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public Guid GameStateId { get; set; }
        public GameState GameState { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public Guid? WinnerId { get; set; }
        public Team? Winner { get; set; }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        public Guid? ParentTournamentId { get; set; }
        public Tournament? ParentTournament { get; set; }

        public List<Tournament> QualifierTournaments { get; set; } = new();

        public TournamentType Type { get; set; }
        public TournamentTier Tier { get; set; }

        public float PrizePool { get; set; }

        public string City { get; set; }

        public int Size { get; set; } 

        public int Year { get; set; }
        public int StartingWeek { get; set; }
        public int DurationWeeks { get; set; }

        public int GroupStageDuration { get; set; }
        public bool hasGroupStage => GroupStageDuration > 0;
        public TournamentGroupStageFormat? GroupStageFormat { get; set; }

        public int PlayoffsDuration { get; set; }
        public bool hasPlayoffs => PlayoffsDuration > 0;
        public TournamentPlayoffsFormat? PlayoffsFormat { get; set; }

        public int TournamentDuration => (hasGroupStage ? GroupStageDuration : 0) + (hasPlayoffs ? PlayoffsDuration : 0);

        public List<Series> Series { get; set; } = new();
        public List<Team> ParticipatingTeams { get; set; } = new();

        public static class VRSPoints
        {
            public static double GetBasePoints(TournamentTier tier, TournamentType type)
            {
                double tierMultiplier = tier switch
                {
                    TournamentTier.S => 1.5,
                    TournamentTier.A => 1.2,
                    TournamentTier.B => 1.0,
                    TournamentTier.C => 0.7,
                    _ => 1.0
                };

                double stageBonus = type switch
                {
                    TournamentType.Online => 1.0,
                    TournamentType.StudioLAN => 1.2,
                    TournamentType.StadiumLAN => 1.7,
                    _ => 1.0
                };

                return 100 * tierMultiplier * stageBonus;
            }
        }

    }
}
