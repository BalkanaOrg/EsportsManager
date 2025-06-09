using EsportsManager.EsportsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.Scripts.MainMenu.Factories
{
    public static class SponsorFactory
    {
        static GameDbContext context = GameService.LoadDataBaseInformation();
        static GameState gameState = GameService.TransferGameState();
        private static Random random = new Random();

        public static Dictionary<string, List<string>> sponsorNames = new()
        {
            ["Betting"] = new List<string> { "99Xbet", "1xBet", "Winbet", "Bet99", "Betano", "GG.Bet", "Pinnacle", "Rivalry" },
            ["WebService"] = new List<string> { "Spotify", "SkinClub", "Twitch", "YouTube", "Kick", "OnlySkins", "Cloudflare", "NordVPN" },
            ["Crypto"] = new List<string> { "Coinbase", "FTX", "Coindesk", "Crypto.com", "Binance", "Bitget", "Kraken", "OKX" },
            ["Car"] = new List<string> { "Honda", "Skoda", "KIA", "Hyundai", "Audi", "BMW", "Mercedes", "Porsche", "Nissan", "Toyota", "Lamborghini" },
            ["Hardware"] = new List<string> { "Intel", "Nvidia", "AMD", "NewEgg", "Corsair", "Cooler Master", "ASUS", "MSI", "Gigabyte" },
            ["Peripherals"] = new List<string> { "Logitech", "Secret Lab", "SteelSeries", "HyperX", "Razer", "Zowie", "Glorious", "Corsair" },
            ["Clothing"] = new List<string> { "H&M", "Zara", "Uniqlo", "Adidas", "Nike", "Puma", "Champion", "Under Armour" },
            ["Luxury"] = new List<string> { "Rolex", "Omega", "Tag Heuer", "Gucci", "Cartier", "Hublot", "Versace" },
            ["LuxuryClothing"] = new List<string> { "Ralph Lauren", "Louis Vuitton", "Balenciaga", "Armani", "Givenchy", "Dolce & Gabbana" },
            ["Drink"] = new List<string> { "Monster", "Red Bull", "G Fuel", "Prime", "Pepsi", "Coca-Cola", "Rockstar", "Dr Pepper" },
            ["Food"] = new List<string> { "McDonald's", "Burger King", "Wendy's", "KFC", "Domino's", "Subway", "Taco Bell", "Pizza Hut" },
            ["Alcohol"] = new List<string> { "Smirnoff", "Bud Light", "Heineken", "Jack Daniel's", "Absolut", "Guinness", "Corona", "Johnnie Walker" },
            //["City"] = new List<string> { "London", "Berlin", "Paris", "Seoul", "Tokyo", "New York", "Los Angeles", "Sofia", "Madrid", "Copenhagen" },
            ["Airline"] = new List<string> { "Emirates", "Qatar Airways", "Lufthansa", "Delta Air Lines", "United Airlines", "British Airways", "Turkish Airlines", "Singapore Airlines" },
            ["Bank"] = new List<string> { "HSBC", "JPMorgan Chase", "Bank of America", "Citibank", "Deutsche Bank", "Barclays", "BNP Paribas", "Santander" },
        };

        public static List<Sponsorship> GenerateBasicSponsors()
        {
            Array b = Enum.GetValues(typeof(SponsorProduct));
            List<Sponsorship> s = new List<Sponsorship>();
            var keys = sponsorNames.Keys.ToList();
            foreach (var key in keys)
            {
                var values = sponsorNames[key].ToList();
                foreach (var value in values)
                {
                    var a = ReturnSponsorProduct(value);
                    if (a == null) a = GenerateSponsorProduct();
                    var sponsor = new Sponsorship
                    {
                        Id = new Guid(),
                        Name = value,
                        Description = "aaaa",
                        LogoUrl = "Snimkaa",
                        GameState = gameState,
                        Product = (SponsorProduct)a,
                        Tier = (SponsorTier)b.GetValue(random.Next(0, 3))
                    };
                    s.Add(sponsor);
                }
            }
            return s;
        }

        public static Sponsorship GenerateSponsor()
        {
            SponsorProduct sp = GenerateSponsorProduct();
            int tier = 0;
            Array values = Enum.GetValues(typeof(SponsorProduct));

            var sponsorName = GenerateSponsorName(sp);
            tier = random.Next(0, 3);
            if (sponsorName == null)
            {
                sponsorName = GenerateGenericSponsorName(sp); // Custom fallback generator
                tier = random.Next(3,6);
            }
            return new Sponsorship
            {
                Id = new Guid(),
                GameState = gameState,
                Name = sponsorName,
                Product = sp,
                Description = "Da",
                LogoUrl = "snimka.png",
                Tier = (SponsorTier)values.GetValue(tier),
            };
        }

        public static Sponsorship GenerateSponsor(string name, string description, string logourl, SponsorProduct product, SponsorTier tier)
        {
            return new Sponsorship
            {
                Id = new Guid(),
                Name = name,
                Description = description,
                LogoUrl = logourl,
                Tier = tier,
                GameState = gameState,
                Product = product,
            };
        }

        private static string GenerateSponsorName(SponsorProduct product)
        {
            if (!sponsorNames.TryGetValue(product.ToString(), out var names))
                return null!; // or throw if invalid enum

            // Get existing sponsor names from DB for this category
            var existingNames = context.Sponsors
                .Where(s => s.Product == product)
                .Select(s => s.Name)
                .ToHashSet();

            // Filter available names
            var availableNames = names.Where(name => !string.IsNullOrWhiteSpace(name) && !existingNames.Contains(name)).ToList();

            if (availableNames.Count == 0)
                return null!; // No names left, caller should handle this

            return availableNames[random.Next(availableNames.Count)];
        }

        private static string GenerateGenericSponsorName(SponsorProduct product)
        {
            string[] genericPrefixes = { "Ultra", "Neo", "Prime", "Max", "Next", "Fusion" };
            string[] genericSuffixes = { "Corp", "Labs", "Solutions", "Systems", "Works" };

            return $"{genericPrefixes[random.Next(genericPrefixes.Length)]} {product}{genericSuffixes[random.Next(genericSuffixes.Length)]}";
        }
        private static SponsorProduct GenerateSponsorProduct()
        {
            Array values = Enum.GetValues(typeof(SponsorProduct));
            int index = random.Next(values.Length - 1);
            return (SponsorProduct)values.GetValue(index)!;
        }
        private static SponsorProduct? ReturnSponsorProduct(string name)
        {
            if (Enum.TryParse<SponsorProduct>(name, ignoreCase: true, out var result))
            {
                return result;
            }

            return null; // return null if no match found
        }
    }
}
