using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Product.Application;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ProductDbContext _productDbContext;
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, ProductDbContext productDbContext)
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
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                Claim claim = new Claim(
                    type: ClaimTypes.Role,
                    value: model.Email
                    );
                await _userManager.AddClaimAsync(user, claim);
                var result = await _userManager.CreateAsync(user, model.Password);
            }
        }
    }
}
