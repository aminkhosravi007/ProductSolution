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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ProductDbContext _productDbContext;

        public ProductController(IMediator mediator, IUserService userService, IConfiguration configuration, UserManager<User> userManager, ProductDbContext productDbContext
            , SignInManager<User> signInManager, IMapper mapper)
        {
            _mediator = mediator;
            _userService = userService;
            _configuration = configuration;
            _userManager = userManager;
            _productDbContext = productDbContext;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }
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
                return BadRequest("Not found such a product");
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
            if(_productDbContext.Users.Any(u=> u.UserName == model.Email))
            {
                var registeredUser = await _userManager.FindByEmailAsync(model.Email);
               var result = await _signInManager.PasswordSignInAsync(registeredUser, model.Password, false, false);
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
            }
            else
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email

                };
                var claim = new Claim(
                    type: ClaimTypes.Role,
                    value: model.Email
                    );
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) { 
                    foreach(var error in result.Errors)
                    {
                        return BadRequest(error.Description);
                    }
                }
                Thread.Sleep(3000);
                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            }
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
