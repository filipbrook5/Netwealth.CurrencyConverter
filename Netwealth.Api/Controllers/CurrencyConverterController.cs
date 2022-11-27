using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netwealth.Common.Models.Dto;
using Netwealth.Common.Models.View;
using Netwealth.Dal;

namespace Netwealth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ILogger<CurrencyConverterController> _logger;
        private readonly NetwealthDbContext _netwealthDbContext;

        public CurrencyConverterController(ILogger<CurrencyConverterController> logger, NetwealthDbContext netwealthDbContext)
        {
            _logger = logger;
            _netwealthDbContext = netwealthDbContext;
        }

        [HttpGet("GetFrom")]
        public async Task<ActionResult<IEnumerable<SelectListViewModel>>> GetFromCurrencies()
        {
            var fromCurrencyList =
                await _netwealthDbContext.Currencies
                    .Select(c => new { c.FromCurrencyCode, c.FromCurrencyName })
                    .Distinct()
                    .OrderBy(c => c.FromCurrencyName)
                    .ToListAsync();

            var returnCurrencyList = new List<SelectListViewModel>();
            if (fromCurrencyList.Any())
            {
                foreach (var currency in fromCurrencyList)
                {
                    returnCurrencyList.Add(
                        new SelectListViewModel
                        { 
                            key = currency.FromCurrencyCode, 
                            value = currency.FromCurrencyName
                        });
                }
            }
            else
            {
                return NotFound();
            }

            return returnCurrencyList;
        }

        [HttpGet("GetTo")]
        public async Task<ActionResult<IEnumerable<SelectListViewModel>>> GetToCurrencies([FromQuery] string fromCurrencyCode)
        {
            var toCurrencyList =
                await _netwealthDbContext.Currencies
                    .OrderBy(c => c.ToCurrencyName)
                    .Where(c => c.FromCurrencyCode == fromCurrencyCode)
                    .Select(c => new { c.ToCurrencyCode, c.ToCurrencyName })
                    .ToListAsync();

            var returnCurrencyList = new List<SelectListViewModel>();
            if (toCurrencyList.Any())
            {
                foreach (var currency in toCurrencyList)
                {
                    returnCurrencyList.Add(
                        new SelectListViewModel
                        {
                            key = currency.ToCurrencyCode,
                            value = currency.ToCurrencyName
                        });
                }
            }
            else
            {
                return NotFound();
            }

            return returnCurrencyList;
        }

        [HttpGet("Convert")]
        public async Task<ActionResult<ConversionResultDto>> ConvertCurrency([FromQuery] string fromCurrencyCode, string toCurrencyCode, decimal fromAmount)
        {
            var currency =
                await _netwealthDbContext.Currencies
                    .Where(c => c.FromCurrencyCode.Equals(fromCurrencyCode) && c.ToCurrencyCode.Equals(toCurrencyCode))
                    .FirstOrDefaultAsync();

            var conversionResultDto =
                new ConversionResultDto() {
                    fromCurrencyCode = fromCurrencyCode,
                    toCurrencyCode = toCurrencyCode,
                    fromAmount= fromAmount,
                    conversionRate = currency.Rate,
                    toAmount = fromAmount * currency.Rate
                };

            return conversionResultDto;
        }
    }
}