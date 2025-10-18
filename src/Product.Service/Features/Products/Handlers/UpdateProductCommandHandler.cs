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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IMongoRepository<Items> _productRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint publishEndpoint;
        public UpdateProductCommandHandler(IMapper mapper, IMongoRepository<Items> productRepository, IPublishEndpoint publishEndpoint)
        {

            _mapper = mapper;
            _productRepository = productRepository;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.Id);
            if (product != null)
            {
                _mapper.Map(request.UpdateProductDto, product);
                product.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);
                await publishEndpoint.Publish(new ProductItemUpdated(product.Id, product.Name, product.Description, product.Price, product.StockQuantity));
                return _mapper.Map<ProductDto>(product);
            }
            return null;
        }
    }
}
