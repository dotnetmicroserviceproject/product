
using Product.Service.Entities;
using Product.Service.Features.Products.Dtos;

namespace Product.Service.Features.Products.Mappers
{
    public class ProductMapperProfile : AutoMapper.Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Items, ProductDto>().ReverseMap();
            CreateMap<ProductCreationDto, Items>();
            CreateMap<UpdateProductDto, Items>();

        }
    }
}
