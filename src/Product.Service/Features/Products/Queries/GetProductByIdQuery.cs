using MediatR;
using Product.Service.Features.Products.Dtos;



namespace Product.Service.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public Guid Id { get; set; }

        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
