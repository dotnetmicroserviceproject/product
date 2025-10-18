using MediatR;
using Product.Service.Features.Products.Dtos;


namespace Product.Service.Features.Products.Commands
{
    public class AddProductsCommand : IRequest<ProductDto>
    {
        public AddProductsCommand(ProductCreationDto productCreation)
        {
            productBody = productCreation;
        }

        public ProductCreationDto productBody { get; }
    }
}
