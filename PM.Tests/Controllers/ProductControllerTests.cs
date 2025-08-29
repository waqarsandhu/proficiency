using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PM.API.Controllers;
using PM.Application.Dto;
using PM.Common.Common;
using PM.Common.Dto;
using PM.Common.Interfaces;

namespace PM.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductService> _serviceMock;
        private Fixture _fixture;
        private ProductController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IProductService>();
            _fixture = new Fixture();
            _controller = new ProductController(_serviceMock.Object);
            
        }

        // -------------------------
        // 1. GET ALL PRODUCTS TEST
        // -------------------------
        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenProductsExist()
        {
            // Arrange
            var input = _fixture.Create<GetProductInputDto>();
            var expectedProducts = _fixture.Create<PagedResult<ProductDto>>();
            _serviceMock.Setup(s => s.GetAllAsync(input)).ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetAll(input) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(expectedProducts, result.Value);
            _serviceMock.Verify(s => s.GetAllAsync(input), Times.Once);
        }

        // -------------------------
        // 2. GET PRODUCT BY ID TESTS
        // -------------------------
        [TestMethod]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.GetById(0) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual(Constants.ProductIDErrorMessage, result.Detail());
        }

        [TestMethod]
        public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            long id = 10;
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _controller.GetById(id) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual(string.Format(Constants.ProductIDNotFoundMessage, id), result.Detail());
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var product = _fixture.Create<ProductDto>();
            _serviceMock.Setup(s => s.GetByIdAsync(product.Id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(product.Id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(product, result.Value);
        }

        // -------------------------
        // 3. CREATE PRODUCT TEST
        // -------------------------
        [TestMethod]
        public async Task Create_ShouldReturnCreatedAtAction_WhenProductCreated()
        {
            // Arrange
            var createDto = _fixture.Build<CreateOrUpdateProductDto>()
                        .With(p => p.Price, 100m) // set decimal explicitly
                        .Create();
            var product = _fixture.Build<ProductDto>()
                                  .With(p => p.Name, createDto.Name)
                                  .With(p => p.Price, createDto.Price)
                                  .With(p => p.Stock, createDto.Stock)
                                  .Create();

            _serviceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(product);

            // Act
            var result = await _controller.Create(createDto) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
            Assert.AreEqual(product, result.Value);
        }

        // -------------------------
        // 4. UPDATE PRODUCT TESTS
        // -------------------------
        [TestMethod]
        public async Task Update_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            var dto = _fixture.Build<CreateOrUpdateProductDto>()
                        .With(p => p.Price, 100m) // set decimal explicitly
                        .Create();
            // Act
            var result = await _controller.Update(0, dto) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual(Constants.ProductIDErrorMessage, result.Detail());
        }

        [TestMethod]
        public async Task Update_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var dto = _fixture.Build<CreateOrUpdateProductDto>()
                       .With(p => p.Price, 100m) // set decimal explicitly
                       .Create();

            long id = 10;
            _serviceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(id, dto) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual(string.Format(Constants.ProductIDNotFoundMessage, id), result.Detail());
        }

        [TestMethod]
        public async Task Update_ShouldReturnOk_WhenProductUpdated()
        {
            // Arrange
            var dto = _fixture.Build<CreateOrUpdateProductDto>()
                       .With(p => p.Price, 100m) // set decimal explicitly
                       .Create();

            long id = 1;
            _serviceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(id, dto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        // -------------------------
        // 5. DELETE PRODUCT TESTS
        // -------------------------
        [TestMethod]
        public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Act
            var result = await _controller.Delete(0) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual(Constants.ProductIDErrorMessage, result.Detail());
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            long id = 100;
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual(string.Format(Constants.ProductIDNotFoundMessage, id), result.Detail());
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenProductDeleted()
        {
            // Arrange
            long id = 1;
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);
        }
    }

    // Helper extension to get ProblemDetails.Detail easily
    internal static class ObjectResultExtensions
    {
        public static string Detail(this ObjectResult result)
        {
            if (result.Value is ProblemDetails details)
                return details.Detail;

            return null;
        }
    }
}
