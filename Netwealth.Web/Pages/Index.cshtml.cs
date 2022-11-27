using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Netwealth.Common.Models.Dto;
using Netwealth.Common.Models.View;
using System.Text.Json;

namespace Netwealth.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // TODO: These settings should be retrieved from appsettings.json...
        private readonly string appsettingsApiUrl = "https://localhost:7242/api/CurrencyConverter";
        private readonly string appsettingsDefaultStartFromCurrency = "GBP";
        private readonly string appsettingsDefaultStartToCurrency = "USD";

        [BindProperty]
        public CurrencyViewModel CurrencyViewModel { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            CurrencyViewModel = new CurrencyViewModel();
            CurrencyViewModel.FromCurrencyCode = appsettingsDefaultStartFromCurrency;
            CurrencyViewModel.ToCurrencyCode = appsettingsDefaultStartToCurrency;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateSelectListsAsync(CurrencyViewModel.FromCurrencyCode);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
            {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync(CurrencyViewModel.FromCurrencyCode);
                return Page();
            }

            var jsonConvert = await CallApiAsync(
                $"{appsettingsApiUrl}/Convert?" +
                $"fromCurrencyCode={CurrencyViewModel.FromCurrencyCode}&" +
                $"toCurrencyCode={CurrencyViewModel.ToCurrencyCode}&" +
                $"fromAmount={CurrencyViewModel.FromAmount}");
            if (jsonConvert != null)
            {
                var conversionResultDto = JsonSerializer.Deserialize<ConversionResultDto>(jsonConvert);
                CurrencyViewModel.ConversionRate = conversionResultDto.conversionRate;
                CurrencyViewModel.ToAmount = conversionResultDto.toAmount;
                await PopulateSelectListsAsync(CurrencyViewModel.FromCurrencyCode);
            }

            return Page();
        }

        private async Task PopulateSelectListsAsync(string toCurrencyCode)
        {
            var jsonTo = await CallApiAsync($"{appsettingsApiUrl}/GetFrom");
            if (jsonTo != null)
            {
                CurrencyViewModel.FromCurrencyList = JsonSerializer.Deserialize<List<SelectListViewModel>>(jsonTo);
            }
            var jsonFrom = await CallApiAsync($"{appsettingsApiUrl}/GetTo?fromCurrencyCode={toCurrencyCode}");
            if (jsonFrom != null)
            {
                CurrencyViewModel.ToCurrencyList = JsonSerializer.Deserialize<List<SelectListViewModel>>(jsonFrom);
            }
        }

        private async Task<string?> CallApiAsync(string Url)
        {
            string json = string.Empty;
            
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();

            try
            {
                if (!string.IsNullOrEmpty(Url))
                {
                    json = await client.GetStringAsync(Url);
                }
                else
                {
                    //return NotFound;
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (TaskCanceledException)
            {
                throw;
            }

            return json;
        }
    }
}