using EsportsManager.EsportsManager.Data.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class TeamFactory
    {
        public static Dictionary<string, List<string>> CountryCities = new()
        {
            ["Bulgaria"] = new List<string> { "Sofia", "Plovdiv", "Varna", "Veliko Turnovo", "Vraca", "Montana" },
            ["Germany"] = new List<string> { "Berlin", "Frankfurt", "Munich", "Hamburg", "Cologne", "Munster", "Trier", "Dresden", "Heidelberg" },
            ["UK"] = new List<string> { "London", "Manchester", "Edinburgh", "Oxford", "Newcastle", "Liverpool", "Middleborough" },
            ["France"] = new List<string> { "Paris", "Lyon", "Marseille", "Naouviette", "Nice", "Lille", "Strasbourg", "Bordeaux", "Avignon", "Toulouse", "Montpellier" },
            ["Spain"] = new List<string> { "Madrid", "Barcelona", "Valencia", "Seville", "Zaragoza", "Granada", "Murcia", "Bilbao", "Cordoba" },
            ["Italy"] = new List<string> { "Rome", "Milan", "Naples", "Florence", "Turin", "Palermo", "Modena" },
            ["Sweden"] = new List<string> { "Stockholm", "Gothenburg", "Malmö" },
            ["Denmark"] = new List<string> { "Copenhagen", "Aarhus" },
            ["Norway"] = new List<string> { "Oslo", "Bergen" },
            ["Finland"] = new List<string> { "Helsinki", "Espoo" },
            ["Russia"] = new List<string> { "Moscow", "Saint Petersburg", "Vladivostok", "Chelyabinsk", "Smolensk" },
            ["Ukraine"] = new List<string> { "Kyiv", "Lviv", "Odessa", "Dnipro", "Zaporizhzhia", "Donetsk", "Vinnytsia" },
            ["Poland"] = new List<string> { "Warsaw", "Krakow", "Katowice", "Gdansk" },
            ["Turkey"] = new List<string> { "Ankara", "Istanbul", "Izmir", "Antalya", "Konya", "Adana", "Bursa", "Diyarbakir" },
            ["Brazil"] = new List<string> { "Sao Paulo", "Rio de Janeiro", "Brasilia", "Manaus", "Belem", "Salvador", "Joao Passoa" },
            ["USA"] = new List<string> { "New York", "Los Angeles", "Chicago", "Detroit", "Dallas", "Austin", "Phoenix", "Houston", "San Diego", "Philadelphia" },
            ["Canada"] = new List<string> { "Toronto", "Vancouver", "Montreal", "Winnipeg", "Quebec City", "Calgary" },
            ["China"] = new List<string> { "Beijing", "Shanghai", "Guangzhou", "Guanxi", "Xi An", "Wuhan", "Chungdu" },
            ["South Korea"] = new List<string> { "Seoul", "Busan", "Incheon", "Daegu", "Gwangju", "Daejeon", "Jeonju-si" },
            ["Australia"] = new List<string> { "Sydney", "Melbourne", "Adelaide", "Brisbane", "Gold Coast" },
            ["Netherlands"] = new List<string> { "Amsterdam", "Rotterdam", "Breda", "Maastricht", "Groningen", "Eindhoven" },
            ["Kazakhstan"] = new List<string> { "Astana", "Almaty" },
            ["Serbia"] = new List<string> { "Belgrade", "Novi Sad", "Nis" },
            ["Romania"] = new List<string> { "Bucharest", "Cluj-Napoca", "Brasov", "Lasi", "Constanta", "Timsoara" },
            ["Greece"] = new List<string> { "Athens", "Thessaloniki", "Piraeus", "Patras", "Kavala" },
            ["Albania"] = new List<string> { "Tirana", "Durres" },
            ["Kosovo"] = new List<string> { "Pristina" },
            ["Mexico"] = new List<string> { "Mexico City", "Guadalajara", "Durango", "Monterrey", "San Luis Potosi" },
            ["Argentina"] = new List<string> { "Buenos Aires", "Cordoba" },
            ["Colombia"] = new List<string> { "Bogota", "Medellin" },
            ["Japan"] = new List<string> { "Tokyo", "Osaka", "Kyoto", "Yokohama", "Nagoya", "Fukoka", "Kobe"}
        };

        private static readonly List<string> _nationalities = new()
        {
            "Germany", "Bulgaria", "UK", "France", "Spain", "Italy", "Sweden", "Denmark", "Norway", "Finland",
            "Russia", "Ukraine", "Poland", "Turkey", "Brazil", "USA", "Canada", "China", "South Korea", "Australia",
            "Netherlands", "Kazakhstan", "Serbia", "Romania", "Greece", "Albania", "Kosovo", "Mexico", "Argentina",
            "Columbia", "South Africa", "India", "Japan", "Mongolia", "Vietnam", "Indonesia", "Philippines"
        };

        private static readonly List<string> _EU = new()
        {
            "Germany", "Bulgaria", "UK", "France", "Spain", "Italy", "Sweden", "Denmark", "Norway", "Finland",
            "Russia", "Ukraine", "Poland", "Turkey", "Netherlands", "Kazakhstan", "Serbia", "Romania", "Greece", "Albania", "Kosovo"
        };

        private static readonly List<string> _AM = new()
        {
            "Brazil", "USA", "Canada", "Mexico", "Argentina", "Columbia"
        };

        private static readonly List<string> _AS = new()
        {
            "China", "South Korea", "Australia", "South Africa", "India", "Japan", "Mongolia", "Vietnam", "Indonesia", "Philippines"
        };

        private static readonly Random random = new Random();

        static string country = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
        static string city = CountryCities[country][random.Next(CountryCities[country].Count)]; 
        static string[] animals = { "Dolphins", "Sharks", "Bears", "Ants", "Lions", "Snakes", "Dragons", "Kangaroos", "Mice", "Dogs", "Wildcats", "Tigers", "Falcons", "Eagles", "Wolves", "Hawks", "Rhinos", "Panthers", "Cobras", "Crows", "Bulls", "Scorpions", "Bisons", "Vultures", "Cheetahs", "Foxes", "Ravens", "Raptors", "Owls", "Bats", "Hornets", "Hyenas", "Jaguars" };
        static string[] prefixes = { "Esports", "Gaming", "Pro", "Elite", "Prime", "Alpha", "Omega", "Royal", "Noble"};
        static string[] suffixes = { "Kings", "Legion", "Squad", "Force", "Nation", "Empire", "Dynasty", "Club", "Collective", "Assembly", "Shock", "Defiant", "Eternal", "Hunters", "Charge", "Spark", "Infernal", "Justice", "Spitfire" };
        static string[] adjectives = { "Red", "Blue", "Black", "White", "Golden", "Silver", "Dark", "Light", "Mighty", "Fierce", "Unstoppable", "Shocking", "Liquid", "Solid", "Purple", "Pink", "Violet", "Proud", "Cloudy", "Sunny", "Platinum" };
        static string[] nouns = { "Swords", "Shields", "Warriors", "Kings", "Might", "Valiance", "Cowboys", "Fuel", "Outlaws", "Giants", "Elfs", "Guns", "Samurais", "Guards", "Guardians" };
        static string[] komb = { "Kings of", "Empire of", "Magicians", "Made in" };
        static string[] strongAdjectives = { "Strong", "Mighty", "Fierce", "Shocking", "Liquid", "Solid", "Proud", "Cloudy", "Sunny", "Heroic", "Aurora", "Spirit", "Pain", "Furious", "Stars", "Instinct", "Enemies", "Squad", "Empire", "Fanatics", "Justice", "Shock", "Fire", "Ice", "Samurais", "Warriors" };
        static string[] colors = { "Red", "Blue", "Green", "Grey", "Black", "White", "Bloody", "Dark", "Light", "Purple", "Aqua", "Lime", "Pink", "Golden", "Orange" };
        static string[] BIG = { "International Gaming", "" };

        public static Team CreateUserTeam(GameState gameState, Game game, string name, string tag, string country, string city, int foundingYear, double budget)
        {
            return new Team
            {
                GameStateId = gameState.Id,
                Name = name,
                Tag = tag,
                Budget = budget,
                WeeklyExpense = 0,
                GameId = game.Id,
                HQ_Country = country,
                HQ_City = city,
                FoundingYear = foundingYear,
            };
        }

        public static EsportsManager.Data.Models.Team CreateTeam(GameState gameState, Game game, string? _country, string? _city)
        {
            string name = GenerateTeamName(_city, _country);
            string country = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
            string city = CountryCities[country][random.Next(CountryCities[country].Count)];
            if (random.Next(100) == 50) name = $"Bad News {animals}";
            string tag = GenerateTeamTag(name);
            EsportsManager.Data.Models.Team team = new EsportsManager.Data.Models.Team
            {
                Name = name,
                Tag = tag,
                Budget = random.Next(10000, 100000),
                WeeklyExpense = 0,
                Game = game,
                GameState = gameState,
                FoundingYear = random.Next(2004, gameState.CurrentYear),
                HQ_Country = _country != null ? _country : country,
                HQ_City = _city != null ? _city : city,
            };
            return team;
        }

        public static EsportsManager.Data.Models.Team CreateTeam(Organization org, Game game, string? name)
        {
            string _name = name!=null ? name : GenerateTeamName(org.HQ_City != null ? org.HQ_City : null, org.HQ_Country != null ? org.HQ_Country : null);
            EsportsManager.Data.Models.Team team = new EsportsManager.Data.Models.Team
            {
                Name = _name,
                Tag = GenerateTeamTag(_name),
                Budget = org.Budget,
                WeeklyExpense = 0,
                GameStateId = org.GameStateId,
                GameState = org.GameState,
                HQ_Country = org.HQ_Country,
                HQ_City = org.HQ_City,
                FoundingYear = random.Next(org.FoundingYear,2025),
                OrganizationId = org.Id,
                Organization = org,
                DivisionalLevel = 1,
                Game = game,
            };
            return team;
        }

        public static string GenerateTeamName(string? city, string? region)
        {
            if(region == null)
            {
                region = CountryCities.Keys.ElementAt(random.Next(CountryCities.Count));
            }
            if(city == null)
            {
                if (CountryCities.ContainsKey(region))
                {
                    city = CountryCities[region][random.Next(CountryCities[region].Count)];
                }
            }
            string name = string.Empty;
            
            int r = random.Next(100);
            //Strong singular word
            if (r > 0 && r <= 10)
            {
                if (r == 10)
                {
                    name = strongAdjectives[random.Next(strongAdjectives.Length)].ToUpper();
                }
                else
                {
                    name = strongAdjectives[random.Next(strongAdjectives.Length)];
                }
            }
            //Team X
            else if (r > 10 && r <= 20)
            {
                if (r > 10 && r <= 15)
                {
                    name = $"Team {nouns[random.Next(nouns.Length)]}";
                }
                else
                {
                    name = $"Team {strongAdjectives[random.Next(strongAdjectives.Length)]}";
                }
            }
            //City related
            else if (r > 20 && r <= 65)
            {
                if (r > 20 && r <= 30)
                {
                    name = $"{city} {adjectives[random.Next(adjectives.Length)]}";
                }
                else if (r > 30 && r <= 35)
                {
                    name = $"{city} {animals[random.Next(animals.Length)]}";
                }
                else if (r > 35 && r <= 43)
                {
                    name = $"{komb[random.Next(komb.Length)]} {city}";
                }
                else if (r > 43 && r <= 45)
                {
                    name = $"{city} University";
                }
                else if (r > 45 && r <= 50)
                {
                    name = $"{city} {suffixes[random.Next(suffixes.Length)]}";
                }
                else if (r > 50 && r <= 55)
                {
                    name = $"{prefixes[random.Next(prefixes.Length)]} {city}";
                }
                else if (r > 55 && r < 60)
                {
                    name = $"{city}5";
                }
                else
                {
                    name = $"{city} High";
                }
            }
            //G2 Esports
            else if (r > 65 && r <= 75)
            {
                string letter = ((char)('A' + random.Next(26))).ToString();
                int n = random.Next(10);
                name = $"{letter}{n}";
                if (r > 70)
                {
                    name = $"{letter}{n} Esports";
                }
            }
            //.Domain
            else if (r > 75 && r <= 80)
            {
                string domain = ReturnDomain(region);
                if (r > 75 && r < 78)
                {
                    name = $"{strongAdjectives[random.Next(strongAdjectives.Length)]}{domain}";
                }
                else
                {
                    name = $"{nouns[random.Next(nouns.Length)]}{domain}";
                }
            }
            //Red Canids/Swords/Shields etc.
            else if (r > 80 && r < 95)
            {
                if (r > 80 && r <= 85)
                {
                    name = $"{colors[random.Next(colors.Length)]} {nouns[random.Next(nouns.Length)]}";
                }
                else if(r>85 && r<91)
                {
                    name = $"{colors[random.Next(colors.Length)]} {animals[random.Next(animals.Length)]}";
                }
                else
                {
                    name = $"{colors[random.Next(colors.Length)]} {suffixes[random.Next(suffixes.Length)]}";
                }
            }
            //Random prefix random suffix
            else
            {
                name = $"{prefixes[random.Next(prefixes.Length)]} {suffixes[random.Next(suffixes.Length)]}";
            }



                return name;
        }

        public static string GenerateTeamTag(string name)
        {
            string tag;
            string[] words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 3)
            {
                tag = words[0].Substring(0, 1).ToUpper() + words[1].Substring(0, 1).ToUpper() + words[2].Substring(0, 1).ToUpper();
            }
            else if (words[0] == "Team")
            {
                if(random.Next(20) == 0)
                {
                    tag = words[0].Substring(0, 1).ToUpper() + words[1].Substring(0, 1).ToUpper();
                }
                else
                {
                    tag = words[1].Substring(0, 3).ToUpper();
                }
            }
            else if (words[0].Length==2)
            {
                tag = words[0].Substring(0, 2).ToUpper();
            }
            else
            {
                tag = words[0].Substring(0, 3).ToUpper();
            }
            return tag;
        }

        private static string ReturnDomain(string country)
        {
            string domain = country.ToLower() switch
            {
                "usa" => ".us",
                "france" => ".fr",
                "germany" => ".de",
                "bulgaria" => ".bg",
                "kazakhstan" => ".kz",
                "serbia" => ".rs",
                "sweden" => ".se",
                "norway" => ".no",
                "denmark" => ".dk",
                "finland" => ".fi",
                "poland" => ".pl",
                "russia" => ".ru",
                "south korea" => ".kr",
                "canada" => ".ca",
                "mexico" => ".mx",
                "argentina" => ".ar",
                "brazil" => ".br",
                "greece" => ".gr",
                "turkey" => ".tr",
                "ukraine" => ".ua",
                "romania" => ".ro",
                "columbia" => ".co",
                "india" => ".in",
                "australia" => ".au",
                "south africa" => ".za",
                "china" => ".cn",
                "japan" => ".jp",
                "uk" => ".gb",
                "spain" => ".es",
                "netherlands" => ".nl",
                "italy" => ".it",
                "albania" => ".al",
                "kosovo" => ".xk",
                "mongolia" => ".mn",
                "indonesia" => ".id",
                "vietnam" => ".vn",
                "philippines" => ".ph",
                _ => ".com"
            };
            return domain;
        }
    }
}
