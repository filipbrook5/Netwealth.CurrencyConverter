namespace Netwealth.Common.Models.Data
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string FromCurrencyCode { get; set; }
        public string FromCurrencyName { get; set; }
        public string ToCurrencyCode { get; set; }
        public string ToCurrencyName { get; set; }
        public decimal Rate { get; set; }
        public decimal InverseRate { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}