using AutoMapper;
using MediatR;
using Product.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductModelDTO>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<ProductModelDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductById(request.id);
            var result = _mapper.Map<ProductModelDTO>(product);
            return result;
        }
    }
}
