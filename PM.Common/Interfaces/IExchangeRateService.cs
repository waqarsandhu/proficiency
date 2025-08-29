using PM.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Common.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse?> GetLatestRatesAsync(string baseCurrency = "USD");
    }
}
