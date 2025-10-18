using MediatR;
using Product.Service.Features.Products.Dtos;



namespace Product.Service.Features.Products.Commands
{
    public class DeleteProductByIdCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; }
        public DeleteProductByIdCommand(Guid productId)
        {
            ProductId = productId;
        }
    }
}
