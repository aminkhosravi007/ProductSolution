using FluentAssertions;
using Moq;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Product.Application.Test
{
    public class ProductCommandHandlerTest
    {
        [Fact]
        public void ProductHandler_CheckCommands()
        {
            // Arrange
            var product = new Domain.ProductModel
            {
                Id = 1,
                Name = "Test Product",
                ManufactureId = 1,
                IsAvailable = true,
                IssuedAdminToken = null,
                ProductDate = DateTime.Now.ToShortDateString()
            };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.AddProduct(product)).ReturnsAsync(product);
            productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).ReturnsAsync(product);
            productRepositoryMock.Setup(repo => repo.UpdateProduct(product)).ReturnsAsync(product);
            //productRepositoryMock.Setup(repo => repo.DeleteProduct(product.Id)).ReturnsAsync();

            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            var createResult = productService.AddProduct(product);
            var readResult = productService.GetProductById(product.Id);
            var updateResult = productService.UpdateProduct(product);
            //var deleteResult = productService.DeleteProduct(product.Id);

            // Assert
            createResult.Should().Be(readResult);
            readResult.Should().BeEquivalentTo(product);
            updateResult.Should().BeSameAs(product);
            //deleteResult.Should().BeTrue();
        }

    }
}
