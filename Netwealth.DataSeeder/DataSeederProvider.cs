using Microsoft.Extensions.DependencyInjection;
using Netwealth.Common.Models.Data;
using Netwealth.Dal;
using System.Text.Json;


namespace Netwealth.DataSeeder
{
    public class DataSeederProvider
    {
        private static NetwealthDbContext _context;

        public DataSeederProvider(IServiceProvider services)
        {
            /*
             * Database context
             */
            _context = services.GetRequiredService<NetwealthDbContext>();
        }

        public void PerformDataSeeding()
        {
            if (_context.Currencies.Count() == 0)
            {
                string workingDirectory = Environment.CurrentDirectory;
                string solutionFolder = Directory.GetParent(workingDirectory).FullName;

                string path = $"{solutionFolder}\\Netwealth.DataSeeder\\SourceData";

                // Please forgive hardcoding of filenames... a short cut to speed up data seeding...
                string[] filenames = new string[4] { "fromEUR.json", "fromGBP.json", "fromPLN.json", "fromUSD.json" };

                foreach (var file in filenames)
                {
                    string json = File.ReadAllText(Path.Combine(path, file));
                    var currencyList = JsonSerializer.Deserialize<FromCurrency>(json);

                    foreach (var item in currencyList.toCurrencies)
                    {
                        var _currency =
                            new Currency
                            {
                                FromCurrencyCode = currencyList.fromCurrencyCode,
                                FromCurrencyName = currencyList.fromCurrencyName,
                                ToCurrencyCode = item.alphaCode,
                                ToCurrencyName = item.name,
                                Rate = item.rate,
                                InverseRate = item.inverseRate,
                                DateUpdated = DateTime.TryParse(item.date, out DateTime latestDate) ? latestDate : DateTime.MinValue
                            };
                        _context.Currencies.Add(_currency);
                    }
                    _context.SaveChanges();
                }
            }
        }
    }
}
