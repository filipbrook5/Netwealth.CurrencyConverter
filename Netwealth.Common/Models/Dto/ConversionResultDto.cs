namespace Netwealth.Common.Models.Dto
{
    public class ConversionResultDto
    {
        public string fromCurrencyCode { get; set; }
        public string toCurrencyCode { get; set; }
        public decimal? fromAmount { get; set; }
        public decimal? conversionRate { get; set; }
        public decimal? toAmount { get; set; }
    }
}
