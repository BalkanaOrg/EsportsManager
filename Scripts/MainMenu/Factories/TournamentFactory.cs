using EsportsManager.EsportsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class TournamentFactory
    {
        private static readonly Random random = new Random();
        private static GameState _gs;

        private static Dictionary<string, List<string>> CountryCities = TeamFactory.CountryCities;

        public static Tournament CreateTournament(GameState gameState, Organization organization, Game game, TournamentType type, TournamentTier tier, float prizePool, int size, int groupStage, int playoffs, TournamentGroupStageFormat? gsformat, TournamentPlayoffsFormat? pformat)
        {
            _gs = gameState;
            int a = random.Next(1, 10);
            string country = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
            string city = CountryCities[country][random.Next(CountryCities[country].Count)];
            string _city;
            if (a == 1) _city = city;
            else _city = organization.HQ_City;

            //Prize pool
            float _prizePool = 100000;
            if (tier == TournamentTier.S) prizePool = 1000000;
            else if (tier == TournamentTier.A) prizePool = 500000;
            else if (tier == TournamentTier.B) prizePool = 250000;
            else if (tier == TournamentTier.C) prizePool = 100000;
            if (type == TournamentType.StadiumLAN) prizePool += 250000;
            if(prizePool==0) prizePool = _prizePool;


            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = TournamentNameGenerator(organization, country, _city, type),
                GameState = gameState,
                Organization = organization,
                Game = game,
                Type = type,
                Tier = tier,
                PrizePool = prizePool,
                City = _city,
                Size = size,
                Year = gameState.CurrentYear,
                StartingWeek = gameState.CurrentWeek,
                DurationWeeks = groupStage+playoffs,
                GroupStageDuration = groupStage,
                PlayoffsDuration = playoffs,
                GroupStageFormat = gsformat,
                PlayoffsFormat = pformat
            };
        }
        //public static Tournament CreateTournament(GameState gameState, Organization organization, Game game, string name, float? prizePool, int? size, int? groupStage, int? playoffs, TournamentGroupStageFormat? gsformat, TournamentPlayoffsFormat? pformat)
        //{
        //    _gs = gameState;

        //    var thisTournamentBefore = gameState.AllTournaments
        //        .Where(t => t.OrganizationId == organization.Id && t.GameStateId == gameState.Id)
        //        .ToList();
        //    int a = random.Next(1, 10);
        //    string country = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
        //    string city = CountryCities[country][random.Next(CountryCities[country].Count)];
        //    string _city;
        //    if (a == 1) _city = city;
        //    else _city = organization.HQ_City;
        //    //Prize pool
        //    float _prizePool = 100000;
        //    if (tier == TournamentTier.S) prizePool = 1000000;
        //    else if (tier == TournamentTier.A) prizePool = 500000;
        //    else if (tier == TournamentTier.B) prizePool = 250000;
        //    else if (tier == TournamentTier.C) prizePool = 100000;
        //    if (type == TournamentType.StadiumLAN) prizePool += 250000;
        //    if (prizePool == 0) prizePool = _prizePool;
        //}

        public static Tournament CreateCSMajor(GameState gameState, Organization organization, Game game, int startingWeek)
        {
            _gs = gameState;
            int a = random.Next(1, 10);
            string country = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
            string city = CountryCities[country][random.Next(CountryCities[country].Count)];
            string _city;
            if (a == 1) _city = city;
            else _city = organization.HQ_City;

            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = $"{organization.Name} {_city} {_gs.CurrentYear} Major",
                GameState = gameState,
                Organization = organization,
                Game = game,
                Type = TournamentType.StadiumLAN,
                Tier = TournamentTier.Major,
                PrizePool = 1250000,
                City = _city,
                Size = 32,
                Year = gameState.CurrentYear,
                StartingWeek = startingWeek,
                DurationWeeks = 4,
                GroupStageDuration = 3,
                PlayoffsDuration = 1,
                GroupStageFormat = TournamentGroupStageFormat.Swiss,
                PlayoffsFormat = TournamentPlayoffsFormat.SingleElimination
            };
        }

        public static Tournament CreateCSMajorRMR(GameState gameState, Organization organization, Game game, Tournament major, string region, TournamentGroupStageFormat format)
        {
            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = $"{major.Name} {region} RMR",
                GameState = gameState,
                Organization = organization,
                Game = game,
                Type = TournamentType.StadiumLAN,
                Tier = TournamentTier.RMR,
                PrizePool = 250000,
                City = major.City,
                Year = major.Year,
                StartingWeek = major.StartingWeek,
                DurationWeeks = 2,
                GroupStageDuration = 2,
                PlayoffsDuration = 0,
                GroupStageFormat = format,
            };
        }



        public static string TournamentNameGenerator(Organization org, string country, string city, TournamentType type)
        {
            string[] tournamentBranding = { "Invitational", "Championship", "Masters", "Conquerors", "Prime", "Kings", "Showoff", "Faceoff", "Bad blood", "Mania" };
            string branding = "";
            var tournaments = _gs.AllTournaments
                .Where(t => t.OrganizationId == org.Id && t.GameStateId == _gs.Id)
                .ToList();
            int a = random.Next(10);
            if(a == 0)
            {
                if(type == TournamentType.Online)
                {
                    branding = $"{org.Name} Online {_gs.CurrentYear}";
                }
                else
                {
                    branding = $"{org.Name} {city} {_gs.CurrentYear}";
                }
                return branding;
            }
            else if(a==1)
            {
                string s = "";
                if (type == TournamentType.Online) s = "Online";
                else if (type == TournamentType.StadiumLAN) s = "LAN";
                else if (type == TournamentType.StudioLAN) s = "Mixed";
                else s = "Tournament";
                branding = $"{org.Name} {s}";
                branding = TournamentSeasonGenerator(branding, org);
                return branding;
            }
            else
            {
                string s = "";
                a = random.Next(3);
                if (a == 0) s = "Season";
                else if (a == 1) s = "Series";
                string temp = $"{org.Name} {tournamentBranding[random.Next(tournamentBranding.Length)]} {s}";
                branding = TournamentSeasonGenerator(temp, org);
                return branding;
            }
        }
        public static string TournamentSeasonGenerator(string branding, Organization org)
        {
            var tournaments = _gs.AllTournaments
                .Where(t => t.OrganizationId == org.Id && t.GameStateId == _gs.Id)
                .ToList();


            if (tournaments.Any(t => t.Name.Contains(branding)))
            {
                int i = 1;
                while (tournaments.Any(t => t.Name == $"{branding} {i}"))
                {
                    i++;
                }
                branding = $"{branding} {i}";
            }
            else
            {
                branding = $"{branding} 1";
            }
            return branding;
        }
    }
}
