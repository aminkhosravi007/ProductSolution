using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Product.Application;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ProductDbContext _productDbContext;
        public UserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ProductDbContext productDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _productDbContext = productDbContext;
        }

        public async Task LoginUser(UserModel model)
        {
            if(model == null) throw new ArgumentNullException("model");
            if(_productDbContext.Users.Any(c=> c.Email == model.Email))
            {
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            }
            else
            {
                var user = new IdentityUser { Email = model.Email };
                Claim claim = new Claim(
                    type: ClaimTypes.Role,
                    value: model.Email
                    );
                await _userManager.AddClaimAsync(user, claim);
                await _userManager.CreateAsync(user, model.Password);
            }
        }
    }
}
