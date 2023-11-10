using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Product.API.Controllers;
using Product.Application;
using Product.Domain;
using Xunit;

namespace Product.UnitTest.Systems.Controllers
{
    public class TestProductController
    {
        private readonly IProductService _productService;
        private readonly ProductController _productController;
        public TestProductController(IProductService productService, ProductController productController)
        {
            _productService = productService;
            _productController = productController;
        }
        [Fact]
        public async Task ProductController_GetAllProducts_ReturnStatusCodeOk()
        {
            //Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(service => service.GetAllProducts())
                .ReturnsAsync(new List<ProductModel>());
            ////Act
            var result = _productController.GetAllProducts();

            ////Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Theory]
        [ClassData(typeof(ProductModelDTO))]
        public async Task ProductController_AddProduct_ReturnCreatedAtRoute(ProductModelDTO productModel)
        {

        }

    }
}