using AutoMapper;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProductModel, ProductModelDTO>().ReverseMap();
        }
    }
}
