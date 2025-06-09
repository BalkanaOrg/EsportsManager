using EsportsManager.EsportsManager.Data.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class OrgFactory
    {
        private static readonly Random random = new Random();

        private static Dictionary<string, List<string>> CountryCities = TeamFactory.CountryCities;

        public static Organization CreateTeamOrganization(GameState gameState, string name, string HQcountry, string HQcity, int foundingYear, float budget)
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                Name = name,
                Budget = budget,
                GameStateId = gameState.Id,
                GameState = gameState,
                Type = OrganizationType.Team,
                HQ_Country = HQcountry,
                HQ_City = HQcity,
                FoundingYear = foundingYear
            };
        }

        public static Organization CreateRandomOrganization(GameState gameState, OrganizationType type)
        {
            string country = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
            string city = CountryCities[country][random.Next(CountryCities[country].Count)];
            switch (type)
            {
                case OrganizationType.Team:
                    return new Organization
                    {
                        Id = Guid.NewGuid(),
                        Name = GenerateTeamOrgName(country, city),
                        Budget = random.Next(100000, 1000000),
                        Type = OrganizationType.Team,
                        GameStateId = gameState.Id,
                        GameState = gameState,
                        HQ_Country = country,
                        HQ_City = city,
                        FoundingYear = random.Next(2010,2025)
                    };
                    break;
                case OrganizationType.TO:
                    return new Organization
                    {
                        Id = Guid.NewGuid(),
                        Name = GenerateTournamentOrgName(),
                        Budget = random.Next(1000000, 10000000),
                        Type = OrganizationType.TO,
                        GameState = gameState,
                        HQ_Country = country,
                        HQ_City = city,
                        FoundingYear = random.Next(2010, 2025)
                    };
                break;
                default:
                    return new Organization
                    {
                        Id = Guid.NewGuid(),
                        Name = GenerateTeamOrgName(country, city),
                        Budget = random.Next(1000000, 10000000),
                        GameStateId = gameState.Id,
                        GameState = gameState,
                        HQ_Country = country,
                        HQ_City = city,
                        FoundingYear = random.Next(2010, 2025)
                    };
                break;
            }
        }

        public static Organization CreateOrganizationFromTeam(GameState gameState, Team team)
        {
            Organization org = new Organization
            {
                Id = Guid.NewGuid(),
                Name = team.Name,
                Budget = team.Budget,
                WeeklyExpense = team.WeeklyExpense,
                GameStateId = gameState.Id,
                GameState = gameState,
                isUserControlled=team.isUserControlled,
                Type = OrganizationType.Team,
                FoundingYear = gameState.CurrentYear,
                Prestige = 100 + team.Prestige/10,
                HQ_Country = team.HQ_Country,
                HQ_City = team.HQ_City
            };
            return org;

        }



        private static string GenerateTournamentOrgName()
        {
            string[] TOprefixes = { "Elite", "Gaming", "Esports" };
            string[] TOsuffixes = { "Series", "Warriors", "Champions", "Dynasty", "Squad" };
            string[] Stier_TO = { "PGL", "ESL", "Blast", "FaceIt", "Skyesports", "Dreamhack", "StarLadder" };
            string[] Online_TO = { "Gamers8", "Onliners", "Balkana", "ESEA", "Challengers", "United", "Sparen" };
            int pattern = random.Next(0, 3);
            if(pattern==0)
            {
                pattern = random.Next(0, 3);
                if(pattern==0)
                {
                    return $"{Stier_TO[random.Next(Stier_TO.Length)]}";
                }
                else
                {
                    return $"{Online_TO[random.Next(Stier_TO.Length)]}";
                }
            }
            else
            {
                return $"{TOprefixes[random.Next(TOprefixes.Length)]} {TOsuffixes[random.Next(TOsuffixes.Length)]}"; // Stier TO
            }
        }
        private static string GenerateTeamOrgName(string? _country, string? _city)
        {
            return TeamFactory.GenerateTeamName(_city, _country);
        }
    }
}
