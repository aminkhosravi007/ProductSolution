using AutoMapper;
using MediatR;
using Product.Application.Commands;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Handlers
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductModelDTO>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<ProductModelDTO> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<ProductModel>(request.product);
            await _productService.AddProduct(model);
            return request.product;
        }
    }
}
