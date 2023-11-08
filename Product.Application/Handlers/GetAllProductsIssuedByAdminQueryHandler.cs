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
    public class GetAllProductsIssuedByAdminQueryHandler : IRequestHandler<GetAllProductsIssuedByAdminQuery, List<ProductModelDTO>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetAllProductsIssuedByAdminQueryHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<List<ProductModelDTO>> Handle(GetAllProductsIssuedByAdminQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetProductsIssuedByAdmin(request.adminEmail);
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
