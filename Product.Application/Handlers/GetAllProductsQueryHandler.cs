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
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductModelDTO>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<List<ProductModelDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProducts();
            List<ProductModelDTO> result = new List<ProductModelDTO>();
            foreach (var product in products)
            {
                var dto = _mapper.Map<ProductModelDTO>(product);
                result.Add(dto);
            }
            return result;
        }
    }
}
