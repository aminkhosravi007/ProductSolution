using Castle.Core.Configuration;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Product.API.Controllers;
using Product.Application;
using Product.Application.Commands;
using Product.Application.Queries;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.AspNetCore.Http;
namespace Product.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _controller;
        public ProductControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _configurationMock = new Mock<IConfiguration>();
            _userServiceMock = new Mock<IUserService>();
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_configurationMock.Object,
                _userServiceMock.Object, _productServiceMock.Object,_mediatorMock.Object);
        }
        [Fact]
        public async Task ProductController_GetAllProducts_ShouldReturnStatusCode200()
        {
            //arrange
            var expected = new List<ProductModelDTO>();
            _mediatorMock.Setup(m=> m.Send(It.IsAny<GetAllProductsQuery>(), default(CancellationToken))).ReturnsAsync(expected);
            //act
            var result = await _controller.GetAllProducts();
            //assert
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType<List<ProductModelDTO>>();
        }
        [Fact]
        public async Task ProductController_GetProductById_ShouldReturnStatusCode200()
        {
            //arrange
            var expected = new ProductModelDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default(CancellationToken))).ReturnsAsync(expected);
            //act
            var result = await _controller.GetProductById(It.IsAny<int>());
            //assert
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType<ProductModelDTO>();
        }
        [Fact]
        public async Task ProductController_AddProduct_ShouldReturnStatusCode201()
        {
            //arrange
            var expected = new ProductModelDTO
            {
                Id = 1,
                Name = "test",
                IsAvailable = true,
                ManufactureId = 1
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddProductCommand>(), default(CancellationToken))).ReturnsAsync(expected);
            //act
            var result = await _controller.AddProduct(expected);
            //assert
            var okResult = result as ObjectResult;
            okResult?.StatusCode.Should().Be(201);
            okResult?.Value.Should().BeOfType<ProductModelDTO>();
        }
        [Fact]
        public async Task ProductController_UpdateProduct_ShouldReturnStatusCode201()
        {
            //arrange
            var dto = new ProductModelDTO
            {
                Id = 11,
                IsAvailable = true,
                ManufactureId = 1,
                Name = "glass"

            };
            _productServiceMock.Setup(m => m.GetProductById(It.IsAny<int>())).ReturnsAsync(new ProductModel()
            {
                Id = 11, Name = "glass" ,IsAvailable = true, IssuedAdminToken = "ali@example.com", ManufactureId = 1, ProductDate = "11/8/2023"
            });
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default(CancellationToken))).ReturnsAsync(new ProductModelDTO
            {
                Id = 11,
                IsAvailable = true,
                ManufactureId = 1,
                Name = "glass"
            });

            //act
            var result = await _controller.UpdateProduct(dto);
            //assert
            var okResult = result as ObjectResult;
            okResult?.StatusCode.Should().Be(201);
            okResult?.Value.Should().BeOfType<ProductModelDTO>();
        }
        [Fact]
        public async Task ProductController_DeleteProduct_ShouldReturnStatusCode204()
        {
            //arrange
            _productServiceMock.Setup(m => m.GetProductById(It.IsAny<int>())).ReturnsAsync(new ProductModel()
            {
                Id = 11,
                Name = "glass",
                IsAvailable = true,
                IssuedAdminToken = "ali@example.com",
                ManufactureId = 1,
                ProductDate = "11/8/2023"
            });
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default(CancellationToken))).ReturnsAsync(new ProductModelDTO
            {
                Id = 11,
                IsAvailable = true,
                ManufactureId = 1,
                Name = "glass"
            });

            //act
            var result = await _controller.DeleteProduct(It.IsAny<int>());
            //assert
            var okResult = result as NoContentResult;
            okResult?.StatusCode.Should().Be(204);
        }
    }
}
