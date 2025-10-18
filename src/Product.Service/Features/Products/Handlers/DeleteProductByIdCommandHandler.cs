using AutoMapper;
using common.MongoDB.Interface;
using MassTransit;
using MediatR;
using Product.Contracts;
using Product.Service.Entities;
using Product.Service.Features.Products.Commands;
using Product.Service.Features.Products.Dtos;



namespace Product.Service.Features.Products.Handlers
{
    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, ProductDto>
    {
        private readonly IMongoRepository<Items> _productRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint publishEndpoint;

        public DeleteProductByIdCommandHandler(IMapper mapper, IMongoRepository<Items> productRepository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            this.publishEndpoint = publishEndpoint;
        }
        public async Task<ProductDto> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);

            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
                await publishEndpoint.Publish(new ProductItemDeleted(product.Id));

                return _mapper.Map<ProductDto>(product);
            }

            return null;
        }
    }
}
