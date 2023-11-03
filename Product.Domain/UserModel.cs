using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain
{
    public class UserModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}
