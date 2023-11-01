using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Commands
{
    public record AddProductCommand(ProductModelDTO product) : IRequest<ProductModelDTO>
    {
    }
}
