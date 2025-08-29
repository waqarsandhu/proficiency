using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PM.Common.Dto;
using PM.Common.Interfaces;

namespace PM.Application.Services
{
    public class ExchangeRateService: IExchangeRateService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey;
        private readonly string _BaseUrl;
        public ExchangeRateService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _BaseUrl = configuration["OpenExchangeRates:BaseUrl"];
            _apiKey = configuration["OpenExchangeRates:ApiKey"];
        }

        public async Task<ExchangeRateResponse?> GetLatestRatesAsync(string baseCurrency = "USD")
        {
            var url = $"{_BaseUrl}/latest.json?app_id={_apiKey}&base={baseCurrency}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRateResponse>(json);
        }
    }
}
