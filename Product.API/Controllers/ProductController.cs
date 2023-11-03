using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Product.Application;
using Product.Application.Commands;
using Product.Application.Queries;
using Product.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public ProductController(IMediator mediator, IUserService userService, IConfiguration configuration)
        {
            _mediator = mediator;
            _userService = userService;
            _configuration = configuration;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }
        [HttpGet("{id}", Name ="GetProduct")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if(product == null) {
                return NotFound("Not found such a product");
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductCommand command)
        {
            if(command == null)
            {
                return BadRequest("Invalid data!");
            }
            await _mediator.Send(command);
            return CreatedAtRoute("GetProduct", new {id = command.product.Id}, command.product);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(command.product.Id));
            if(product == null)
            {
                return BadRequest("Not found such a product");
            }
            await _mediator.Send(command);
            return CreatedAtRoute("GetProduct", new { id = command.product.Id }, command.product);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if(product == null)
            {
                return NotFound("Not found such a product");
            }
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IActionResult Login(UserModel model)
        {
            _userService.LoginUser(model);
            return Ok(new {token = CreateToken()});
        }
        [AllowAnonymous]
        private string CreateToken()
        {
            
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddMinutes(5)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
