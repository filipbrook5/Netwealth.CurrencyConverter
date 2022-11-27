namespace Netwealth.DataSeeder
{
    internal class FromCurrency
    {
        public string? fromCurrencyCode { get; set; }
        public string? fromCurrencyName { get; set; }
        public List<ToCurrency>? toCurrencies { get; set; }
    }
}
