using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netwealth.Common.Models.View
{
    public class CurrencyViewModel
    {
        [DisplayName("From currency")]
        public string FromCurrencyCode { get; set; }

        public List<SelectListViewModel>? FromCurrencyList { get; set; }

        [DisplayName("To currency")]
        public string ToCurrencyCode { get; set; }

        public List<SelectListViewModel>? ToCurrencyList { get; set; }

        [Required(ErrorMessage = "Please enter the amount you would like to be exchanged")]
        [DisplayName("Amount to be converted")]
        public decimal? FromAmount { get; set; }

        [DisplayName("Exchange rate")]
        public decimal? ConversionRate { get; set; }

        [DisplayName("Converted amount")]
        public decimal? ToAmount { get; set; }
    }
}
