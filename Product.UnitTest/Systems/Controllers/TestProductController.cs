using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Product.API.Controllers;

namespace Product.UnitTest.Systems.Controllers
{
    public class TestProductController
    {
        [Fact]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            //Arrange
            var test = new ProductController();
            //Act
            var result = (ObjectResult) await test.GetAllProducts();
            //Assert
            result.StatusCode.Should().Be(200);
        }
    }
}