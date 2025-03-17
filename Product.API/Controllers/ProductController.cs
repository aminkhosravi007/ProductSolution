using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public ProductController(IConfiguration configuration, IUserService userService, IProductService productService,IMediator mediator)
        {
            _configuration = configuration;
            _userService = userService;
            _mediator = mediator;
            _productService = productService;
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
        public async Task<IActionResult> AddProduct([FromBody] ProductModelDTO dto)
        {
            var command = new AddProductCommand(dto);
            if(dto == null)
            {
                return BadRequest("Invalid data!");
            }
            var createdProduct = await _mediator.Send(command);
            return CreatedAtRoute("GetProduct", new {id = createdProduct.Id}, createdProduct);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModelDTO dto)
        {
            var command = new UpdateProductCommand(dto);
            var product = await _mediator.Send(new GetProductByIdQuery(dto.Id));
            var dbProduct = await _productService.GetProductById(dto.Id);
            if (product == null)
            {
                return NotFound("Not found such a product");
            }
            else if (dbProduct?.IssuedAdminToken == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value)
            {
                var updatedProduct = await _mediator.Send(command);
                return CreatedAtRoute("GetProduct", new { id = updatedProduct.Id }, updatedProduct);
            }
            return Unauthorized();
            

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            var dbProduct = await _productService.GetProductById(id);
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
            //if(_productDbContext.Users.Any(u=> u.UserName == model.Email))
            //{
            //    var registeredUser = await _userManager.FindByEmailAsync(model.Email);
            //    var result = await _signInManager.PasswordSignInAsync(registeredUser, model.Password, false, false);
            //    if (!result.Succeeded) {
            //        return BadRequest(result);
            //    }
            //}
            //else
            //{
            //    var user = new User
            //    {
            //        UserName = model.Email,
            //        Email = model.Email

            //    };
            //    var result = await _userManager.CreateAsync(user, model.Password);
            //    if (!result.Succeeded) { 
            //        foreach(var error in result.Errors)
            //        {
            //            return BadRequest(error.Description);
            //        }
            //    }
            //    Thread.Sleep(3000);
            //    await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            //}
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
