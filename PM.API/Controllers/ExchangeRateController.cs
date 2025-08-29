using Microsoft.AspNetCore.Mvc;
using PM.Application.Services;
using PM.Common.Interfaces;

namespace PM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestRates([FromQuery] string baseCurrency = "USD")
        {
            var result = await _exchangeRateService.GetLatestRatesAsync(baseCurrency);

            if (result == null || result.Rates == null)
                return NotFound("Unable to fetch exchange rates.");

            return Ok(result);
        }
    }
}
