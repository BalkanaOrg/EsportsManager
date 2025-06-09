using EsportsManager.EsportsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class GameFactory
    {
        private static GameState gameState = GameService.TransferGameState();
        private static GameDbContext context = GameService.LoadDataBaseInformation();

        private static Random random = new Random();

        //public static Game CreateGame(Organization developer)
        //{

        //}

        public static Game CreateGame(Organization developer, string fullname, string shortname, string iconurl, GameGenre genre, string description, int? dedicatedPlayers)
        {
            if(dedicatedPlayers == null) dedicatedPlayers = random.Next(150000,1000000);

            var game = new Game
            {
                Developer = developer,
                FullName = fullname,
                ShortName = shortname,
                IconURL = iconurl,
                Genre = genre,
                Description = description,
                DedicatedPlayers = (int)dedicatedPlayers,
                GameState = developer.GameState
            };
            return game;
        }
        public static Organization CreateDeveloper(string? name, string? country, string? city, int? foundingYear)
        {
            if (name == null) name = GenerateDeveloperName();
            if (country == null) country = TeamFactory.CountryCities.Keys.ElementAt(random.Next(TeamFactory.CountryCities.Count));
            if (city == null) city = TeamFactory.CountryCities[country][random.Next(TeamFactory.CountryCities[country].Count)];
            if (foundingYear == null) foundingYear = random.Next(1990, 2020);


            var dev = new Organization
            {
                Id = new Guid(),
                Name = name,
                HQ_City = city,
                HQ_Country = country,
                FoundingYear = (int)foundingYear,
                GameState = gameState,
                Budget = random.Next(900000, 10000000),
                WeeklyExpense = -(random.Next(10000, 100000)),
                isUserControlled = false,
                Type = OrganizationType.Publisher,
                Prestige = random.Next(100,390),
                WeeklyHappiness = 0,
                WeeklyMotivation = 0,
                WeeklyPrestiegeDecay = 0,
            };
            return dev;
        }



        public static string GenerateDeveloperName()
        {
            string[] devNames = { "Valvitto", "Fighter Games", "Storm", "Nuntindo", "Toir", "Balkana Games" };
            string[] FPSprefix = { "Fight", "Fist", "Bullet", "Army", "Deagle", "Sniper" };
            string[] overallPrefix = { "Brain", "Macro", "Frost", "Nightmare", "Chaos", "Nebula", "Nova", "Iron", "Black", "Quantum" };
            string[] suffix = { "Games", "Inc.", "Corp.", "Development", "Studios", "Entertainment", "Interactive", "Works", "Forge", "Soft" };
            string[] symbols = { "& Co.", "™", "®", "" };

            int r = random.Next(100);
            string name = string.Empty;

            if (r < 10)
            {
                // Realistic fake dev name
                name = devNames[random.Next(devNames.Length)];
            }
            else if (r < 25)
            {
                // Prefix + Suffix (e.g., Brain Games)
                name = $"{overallPrefix[random.Next(overallPrefix.Length)]} {suffix[random.Next(suffix.Length)]}";
            }
            else if (r < 35)
            {
                // FPS Style (e.g., Bullet Studios)
                name = $"{FPSprefix[random.Next(FPSprefix.Length)]} {suffix[random.Next(suffix.Length)]}";
            }
            else if (r < 50)
            {
                // Compound prefix (e.g., FrostNova Inc.)
                var prefix1 = overallPrefix[random.Next(overallPrefix.Length)];
                var prefix2 = overallPrefix[random.Next(overallPrefix.Length)];
                name = $"{prefix1}{prefix2} {suffix[random.Next(suffix.Length)]}";
            }
            else if (r < 65)
            {
                // Symbolic (e.g., Macro Studios™)
                name = $"{overallPrefix[random.Next(overallPrefix.Length)]} {suffix[random.Next(suffix.Length)]}{symbols[random.Next(symbols.Length)]}";
            }
            else if (r < 75)
            {
                // Minimalist (e.g., NOVA)
                name = overallPrefix[random.Next(overallPrefix.Length)].ToUpper();
            }
            else if (r < 85)
            {
                // Legacy-style (e.g., BlackForge Interactive)
                name = $"{overallPrefix[random.Next(overallPrefix.Length)]}{suffix[random.Next(suffix.Length)]}";
            }
            else if (r < 95)
            {
                // Short code + Suffix (e.g., G7 Studios)
                string letter = ((char)('A' + random.Next(26))).ToString();
                int num = random.Next(10);
                name = $"{letter}{num} {suffix[random.Next(suffix.Length)]}";
            }
            else
            {
                // Indie vibe (e.g., Sad Skeleton Studios)
                string[] indieAdj = { "Sad", "Happy", "Lazy", "Rusty", "Angry", "Funky" };
                string[] indieNouns = { "Skeleton", "Pixel", "Cowboy", "Duck", "Pixel", "Goblin" };
                name = $"{indieAdj[random.Next(indieAdj.Length)]} {indieNouns[random.Next(indieNouns.Length)]} {suffix[random.Next(suffix.Length)]}";
            }

            return name;
        }

    }
}
