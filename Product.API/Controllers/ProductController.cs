using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Product.Application;
using Product.Application.Commands;
using Product.Application.Queries;
using Product.Domain;
using Product.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly ProductDbContext _productDbContext;
        private readonly IUserService _userService;

        public ProductController(IMediator mediator, IConfiguration configuration, ProductDbContext productDbContext
            , IUserService userService)
        {
            _mediator = mediator;
            _configuration = configuration;
            _productDbContext = productDbContext;
            _userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }
        [AllowAnonymous]
        [HttpGet("GetAllProductsIssuedByAdmin/{adminEmail}")]
        public async Task<IActionResult> GetAllProductsIssuedByAdmin(string adminEmail)
        {
            var products = await _mediator.Send(new GetAllProductsIssuedByAdminQuery(adminEmail));
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
            var dbProduct = await _productDbContext.Products.FindAsync(product.Id);
            if (product == null)
            {
                return NotFound("Not found such a product");
            }
            else if (dbProduct?.IssuedAdminToken == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value)
            {
                await _mediator.Send(command);
                return CreatedAtRoute("GetProduct", new { id = command.product.Id }, command.product);
            }
            return Unauthorized();
            

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            var dbProduct = await _productDbContext.Products.FindAsync(product.Id);
            if (product == null)
            {
                return NotFound("Not found such a product");
            }
            else if (dbProduct?.IssuedAdminToken == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value)
            {
                await _mediator.Send(new DeleteProductCommand(id));
                return NoContent();
            }
            return Unauthorized();
            
        }
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(UserModel model)
        {
            await _userService.LoginUser(model);
            HttpContext.Session.SetString("email", model.Email);

            return Ok(new {token = CreateToken(model.Email)});
        }
        [AllowAnonymous]
        private string CreateToken(string claimValue)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, claimValue)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddMinutes(5)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
