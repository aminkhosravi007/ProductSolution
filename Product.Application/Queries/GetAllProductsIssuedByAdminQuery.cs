using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Queries
{
    public record GetAllProductsIssuedByAdminQuery(string adminEmail): IRequest<List<ProductModelDTO>>
    {
    }
}
