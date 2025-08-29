using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PM.API.Controllers;
using PM.Common.Dto;
using PM.Common.Interfaces;

namespace PM.Tests.Controllers
{
    [TestClass]
    public class ExchangeRateControllerTests
    {
        private IFixture _fixture;
        private Mock<IExchangeRateService> _exchangeRateServiceMock;
        private ExchangeRateController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize AutoFixture with AutoMoq
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            // Mock ExchangeRateService
            _exchangeRateServiceMock = _fixture.Freeze<Mock<IExchangeRateService>>();

            // Inject mock into controller
            _controller = new ExchangeRateController(_exchangeRateServiceMock.Object);
        }

        [TestMethod]
        public async Task GetLatestRates_ShouldReturnOk_WhenRatesAvailable()
        {
            // Arrange
            var response = _fixture.Build<ExchangeRateResponse>()
                                   .With(r => r.Base, "USD")
                                   .With(r => r.Rates, new Dictionary<string, decimal>
                                   {
                                       {"PKR", 300.25M},
                                       {"EUR", 0.91M}
                                   })
                                   .Create();

            _exchangeRateServiceMock
                .Setup(s => s.GetLatestRatesAsync("USD"))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetLatestRates("USD") as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var returnedValue = result.Value as ExchangeRateResponse;
            Assert.IsNotNull(returnedValue);
            Assert.AreEqual("USD", returnedValue.Base);
            Assert.AreEqual(300.25M, returnedValue.Rates["PKR"]);
        }

        [TestMethod]
        public async Task GetLatestRates_ShouldReturnNotFound_WhenRatesAreNull()
        {
            // Arrange
            _exchangeRateServiceMock
                .Setup(s => s.GetLatestRatesAsync("USD"))
                .ReturnsAsync((ExchangeRateResponse)null);

            // Act
            var result = await _controller.GetLatestRates("USD") as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Unable to fetch exchange rates.", result.Value);
        }

        [TestMethod]
        public async Task GetLatestRates_ShouldReturnNotFound_WhenRatesDictionaryIsNull()
        {
            // Arrange
            var response = _fixture.Build<ExchangeRateResponse>()
                                   .With(r => r.Base, "USD")
                                   .Without(r => r.Rates)
                                   .Create();

            _exchangeRateServiceMock
                .Setup(s => s.GetLatestRatesAsync("USD"))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetLatestRates("USD") as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Unable to fetch exchange rates.", result.Value);
        }
    }
}

