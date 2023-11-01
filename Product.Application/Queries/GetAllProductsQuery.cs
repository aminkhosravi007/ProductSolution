using MediatR;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Queries
{
    public class GetAllProductsQuery : IRequest<List<ProductModelDTO>>
    {
    }
}
