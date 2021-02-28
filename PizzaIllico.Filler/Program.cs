using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace PizzaIllico.Filler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var shops = Shops();
            Console.WriteLine(shops.Count);

            var pizzas = Pizzas();
            Console.WriteLine(pizzas.Count);
            
            //await DownloadImages();
            List<byte[]> images = Directory.EnumerateFiles("images", "*.jpg")
                .Select(File.ReadAllBytes)
                .ToList();

            await Insert(shops, pizzas, images);
        }

        private static List<ShopItem> Shops()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            using var reader = new StreamReader("crous.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ";",
                Quote = '"',
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            });

            var items = csv.GetRecords<CrousItem>().ToList();

            double latOrleans = 47.902964;
            double lonOrleans = 1.909251;

            return items.ConvertAll(x => new ShopItem
                {
                    Latitude = x.Lat,
                    Longitude = x.Lon,
                    Name = x.Title,
                    MinutesPerKilometer = Math.Round(random.NextDouble() * 10, 2),
                    Address = ToAddress(x.Contact)
                })
                .Where(x => x.Latitude != 0 && x.Longitude != 0 && !string.IsNullOrEmpty(x.Address) && !string.IsNullOrEmpty(x.Name))
                .Where(x => DistanceInKmBetweenEarthCoordinates(x.Latitude, x.Longitude, latOrleans, lonOrleans) < 100)
                .ToList();

            static string ToAddress(string source)
            {
                int start = source.IndexOf("<p>", StringComparison.InvariantCulture);
                if (start < 0)
                {
                    return null;
                }

                int startPos = start + "<p>".Length;
                int endPos = source.IndexOf("<", startPos, StringComparison.InvariantCulture);

                string res = source.Substring(startPos, endPos - startPos);

                return res;
            }

            static double DegreesToRadians(double degrees)
            {
                return degrees * Math.PI / 180;
            }

            static double DistanceInKmBetweenEarthCoordinates(double lat1, double lon1, double lat2, double lon2)
            {
                var earthRadiusKm = 6371;

                var dLat = DegreesToRadians(lat2 - lat1);
                var dLon = DegreesToRadians(lon2 - lon1);

                lat1 = DegreesToRadians(lat1);
                lat2 = DegreesToRadians(lat2);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return earthRadiusKm * c;
            }
        }

        private static List<CreatePizzaRequest> Pizzas()
        {
            using var reader = new StreamReader("pizza.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ",",
                Quote = '"',
                PrepareHeaderForMatch = args => args.Header.ToLower()
            });

            var items = csv.GetRecords<PizzaItem>().ToList();

            HashSet<string> name = new HashSet<string>();
            HashSet<string> desc = new HashSet<string>();
            return items
                .Where(x => Parse(x.MenusAmountMax) > 1 || Parse(x.MenusAmountMin) > 1)
                .Where(x => !string.IsNullOrWhiteSpace(x.MenusDescription))
                .Where(x => !string.IsNullOrWhiteSpace(x.MenusName))
                .Select(x => new CreatePizzaRequest
                {
                    Description = x.MenusDescription,
                    Name = x.MenusName,
                    Price = Price(Parse(x.MenusAmountMin), Parse(x.MenusAmountMax))
                })
                .Where(x => x.Description.Count(c => c == ',') >= 2)
                .Where(x => name.Add(x.Name) && desc.Add(x.Description))
                .ToList();

            static double Price(double min, double max)
            {
                double value = min < 1 ? max : max < 1 ? min : (max + min) / 2.0;
                return Math.Round(value, 1);
            }

            static double Parse(string i)
            {
                if (string.IsNullOrWhiteSpace(i))
                {
                    return -1;
                }

                if (double.TryParse(i, out double result))
                {
                    return result;
                }

                return -1;
            }
        }

        private static async Task DownloadImages()
        {
            HttpClient client = new HttpClient();
            Random random = new Random(DateTime.Now.Millisecond);

            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }
            
            for (int i = 0; i < 100; ++i)
            {
                int idx = random.Next(10, 6000);
                string url = $"http://pizzagan.csail.mit.edu/pizzaGANdata/images/{idx:D5}.jpg";

                byte[] data = await client.GetByteArrayAsync(url);
                
                File.WriteAllBytes($"images/{idx}.jpg", data);
            }
        }

        private static async Task Insert(List<ShopItem> shops, List<CreatePizzaRequest> pizzas, List<byte[]> images)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            HttpClient client = new HttpClient();
            foreach (ShopItem shop in shops)
            {
                int pizzaCount = random.Next(12, 25);

                HashSet<int> ids = new HashSet<int>();
                while (ids.Count < pizzaCount)
                {
                    ids.Add(random.Next(0, pizzas.Count));
                }

                CreateShopRequest request = new CreateShopRequest
                {
                    Shop = shop,
                    Token = "p7Ds2dGps2UDudkY",
                    Pizzas = ids.Select(x => pizzas[x])
                        .Select(x => new CreatePizzaRequest
                        {
                            Description = x.Description,
                            Name = x.Name,
                            Price = x.Price,
                            Image = images[random.Next(0, images.Count)],
                            OutOfStock = random.NextDouble() < 0.08,
                        }).ToList()
                };

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/v1/shops");
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    continue;
                }

                
            }
        }
        
        private class CrousItem
        {
            public string Title { get; set; }

            public double Lat { get; set; }

            public double Lon { get; set; }

            public string Contact { get; set; }
        }

        private class PizzaItem
        {
            public string MenusAmountMax { get; set; }
            public string MenusAmountMin { get; set; }
            public string MenusDescription { get; set; }
            public string MenusName { get; set; }
        }
    }
}