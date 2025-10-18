using MediatR;
using Product.Service.Features.Products.Dtos;


namespace Product.Service.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public Guid Id { get; }
        public UpdateProductDto UpdateProductDto { get; }
        public UpdateProductCommand(Guid id, UpdateProductDto updateProductDto)
        {
            Id = id;
            UpdateProductDto = updateProductDto;
        }
    }
}
