using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Moq;
using PM.Application.Dto;
using PM.Application.Services;
using PM.Common.Dto;
using PM.Common.Interfaces;
using PM.EntityFrameworkCore.Entities;
using PM.Tests.Common;

namespace PM.Tests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private IFixture _fixture;
        private Mock<IRepository<Product, long>> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _repositoryMock = _fixture.Freeze<Mock<IRepository<Product, long>>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();

            _productService = new ProductService(_repositoryMock.Object, _mapperMock.Object);
        }

        // ✅ TEST 1 - CreateAsync()
        [TestMethod]
        public async Task CreateAsync_ShouldReturnProductDto_WhenProductCreated()
        {
            // Arrange
            var createDto = new CreateOrUpdateProductDto
            {
                Name = "TEST",
                Price = 100,
                Stock = 1
            };

            // Create a product manually to avoid AutoFixture decimal issue
            var product = new Product
            {
                Id = 1,
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock,
                CreatedOn = DateTime.Now
            };

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };

            _mapperMock.Setup(m => m.Map<Product>(createDto)).Returns(product);
            _repositoryMock.Setup(r => r.CreateAndGetAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _productService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productDto, result);
            _repositoryMock.Verify(r => r.CreateAndGetAsync(It.IsAny<Product>()), Times.Once);
        }


        // ✅ TEST 2 - GetAllAsync()
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnPagedResultOfProductDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new Product { Id = 2, Name = "Product 2", Price = 200, Stock = 5 },
                new Product { Id = 3, Name = "Product 3", Price = 300, Stock = 15 }
            };

            var asyncProducts = new TestAsyncEnumerable<Product>(products);

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new ProductDto { Id = 2, Name = "Product 2", Price = 200, Stock = 5 },
                new ProductDto { Id = 3, Name = "Product 3", Price = 300, Stock = 15 }
            };

            var input = new GetProductInputDto();

            _repositoryMock
                .Setup(r => r.GetAll())
                .Returns(asyncProducts);

            _mapperMock
                .Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                .Returns(productDtos);

            // Act
            var result = await _productService.GetAllAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(3, result.Items.Count);
            Assert.AreEqual(productDtos, result.Items);

            _repositoryMock.Verify(r => r.GetAll(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()), Times.Once);
        }

        // ✅ TEST 2.1 - GetAllAsync()
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnFilterProductDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new Product { Id = 2, Name = "Product 2", Price = 200, Stock = 5 },
                new Product { Id = 3, Name = "Product 3", Price = 300, Stock = 15 }
            };

            var asyncProducts = new TestAsyncEnumerable<Product>(products);

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new ProductDto { Id = 2, Name = "Product 2", Price = 200, Stock = 5 },
                new ProductDto { Id = 3, Name = "Product 3", Price = 300, Stock = 15 }
            };

            var input = new GetProductInputDto{ Search = "Product" };

            _repositoryMock
                .Setup(r => r.GetAll())
                .Returns(asyncProducts);

            _mapperMock
                .Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                .Returns(productDtos);

            // Act
            var result = await _productService.GetAllAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(3, result.Items.Count);
            Assert.AreEqual(productDtos, result.Items);

            _repositoryMock.Verify(r => r.GetAll(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()), Times.Once);
        }

        // ✅ TEST 2.2 - GetAllAsync()
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnPaginatedProductDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new Product { Id = 2, Name = "Product 2", Price = 200, Stock = 5 },
                new Product { Id = 3, Name = "Product 3", Price = 300, Stock = 15 }
            };

            var asyncProducts = new TestAsyncEnumerable<Product>(products);

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new ProductDto { Id = 2, Name = "Product 2", Price = 200, Stock = 5 },
                new ProductDto { Id = 3, Name = "Product 3", Price = 300, Stock = 15 }
            };

            var input = new GetProductInputDto { Page = 2, PageSize = 10 };

            _repositoryMock
                .Setup(r => r.GetAll())
                .Returns(asyncProducts);

            _mapperMock
                .Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
                .Returns(productDtos);

            // Act
            var result = await _productService.GetAllAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(3, result.Items.Count);
            Assert.AreEqual(productDtos, result.Items);

            _repositoryMock.Verify(r => r.GetAll(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()), Times.Once);
        }

        // ✅ TEST 3 - GetByIdAsync() - Found
        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnProductDto_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 100,
                Stock = 5
            };

            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Price = 100,
                Stock = 5
            };

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(m => m.Map<ProductDto>(product))
                .Returns(productDto);

            // Act
            var result = await _productService.GetByIdAsync(product.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productDto, result);
            _repositoryMock.Verify(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()), Times.Once);
        }


        // ✅ TEST 4 - GetByIdAsync() - Not Found
        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                           .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetByIdAsync(999);

            // Assert
            Assert.IsNull(result);
        }

        // ✅ TEST 5 - DeleteAsync() - Success
        [TestMethod]
        public async Task DeleteAsync_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 100,
                Stock = 5
            };

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            _repositoryMock
                .Setup(r => r.DeleteAsync(product))
                .Returns(Task.CompletedTask); // Mock delete call

            // Act
            var result = await _productService.DeleteAsync(product.Id);

            // Assert
            Assert.IsTrue(result);
            _repositoryMock.Verify(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(product), Times.Once);
        }


        // ✅ TEST 6 - DeleteAsync() - Not Found
        [TestMethod]
        public async Task DeleteAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                           .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.DeleteAsync(999);

            // Assert
            Assert.IsFalse(result);
        }

        // ✅ TEST 7 - UpdateAsync() - Success
        [TestMethod]
        public async Task UpdateAsync_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Old Product",
                Price = 50,
                Stock = 5
            };

            var updateDto = new CreateOrUpdateProductDto
            {
                Name = "Updated Product",
                Price = 100,
                Stock = 10
            };

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(m => m.Map(updateDto, product))
                .Callback<CreateOrUpdateProductDto, Product>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.Price = src.Price;
                    dest.Stock = src.Stock;
                });

            _repositoryMock
                .Setup(r => r.UpdateAsync(product))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productService.UpdateAsync(product.Id, updateDto);

            // Assert
            Assert.IsTrue(result);
            _mapperMock.Verify(m => m.Map(updateDto, product), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        // ✅ TEST 8 - UpdateAsync() - Not Found
        [TestMethod]
        public async Task UpdateAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var updateDto = new CreateOrUpdateProductDto
            {
                Name = "Updated Product",
                Price = 100,
                Stock = 10
            };

            _repositoryMock
                .Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.UpdateAsync(999, updateDto);

            // Assert
            Assert.IsFalse(result);
            _mapperMock.Verify(m => m.Map(updateDto, It.IsAny<Product>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

    }
}
