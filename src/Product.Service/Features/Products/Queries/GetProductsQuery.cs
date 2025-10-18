using MediatR;
using Product.Service.Features.Products.Dtos;



namespace Product.Service.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
      
    }
}
