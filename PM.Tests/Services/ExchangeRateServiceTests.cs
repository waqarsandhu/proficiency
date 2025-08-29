using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using PM.Application.Services;
using PM.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.AutoMoq;
using AutoFixture;

namespace PM.Tests.Services
{
    [TestClass]
    public class ExchangeRateServiceTests
    {
        private IFixture _fixture;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private ExchangeRateService _service;
        private IConfiguration _configuration;

        [TestInitialize]
        public void Setup()
        {
            // 1. Initialize AutoFixture with AutoMoq customization
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            // 2. In-memory configuration
            var inMemorySettings = new Dictionary<string, string>
            {
                {"OpenExchangeRates:BaseUrl", "https://openexchangerates.org/api"},
                {"OpenExchangeRates:ApiKey", "test_api_key"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // 3. Mock HttpMessageHandler
            _httpMessageHandlerMock = _fixture.Freeze<Mock<HttpMessageHandler>>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // 4. Create service instance using AutoFixture
            _service = new ExchangeRateService(httpClient, _configuration);
        }

        [TestMethod]
        public async Task GetLatestRatesAsync_ShouldReturnRates_WhenApiReturnsSuccess()
        {
            // Arrange
            var mockResponse = _fixture.Build<ExchangeRateResponse>()
                                       .With(x => x.Base, "USD")
                                       .With(x => x.Rates, new Dictionary<string, decimal>
                                       {
                                           {"PKR", 300.25M},
                                           {"EUR", 0.91M}
                                       })
                                       .Create();

            var expectedJson = JsonConvert.SerializeObject(mockResponse);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedJson)
                });

            // Act
            var result = await _service.GetLatestRatesAsync("USD");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("USD", result.Base);
            Assert.AreEqual(300.25M, result.Rates["PKR"]);
            Assert.AreEqual(0.91M, result.Rates["EUR"]);

            // Verify correct API URL & request method
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString().StartsWith("https://openexchangerates.org/api/latest.json")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetLatestRatesAsync_ShouldThrow_WhenApiReturnsUnauthorized()
        {
            // Arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            // Act
            await _service.GetLatestRatesAsync("USD");
        }
    }
}
