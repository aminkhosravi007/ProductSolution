using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public interface IUserRepository
    {
        Task LoginUser(UserModel model);
    }
}
