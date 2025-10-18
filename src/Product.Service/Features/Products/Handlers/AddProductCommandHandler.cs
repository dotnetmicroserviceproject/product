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
    public class AddProductCommandHandler : IRequestHandler<AddProductsCommand, ProductDto>
    {
        private readonly IMongoRepository<Items> _productRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint publishEndpoint;

        public AddProductCommandHandler(IMapper mapper, IMongoRepository<Items> productRepository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            this.publishEndpoint = publishEndpoint;
        }
        public async Task<ProductDto> Handle(AddProductsCommand request, CancellationToken cancellationToken)
        {
            var product = new Items
            {
                Name = request.productBody.Name,
                Description = request.productBody.Description,
                Price = request.productBody.Price,
                StockQuantity = request.productBody.StockQuantity,
                CreatedDate = DateTime.Now,
            };
           
            await _productRepository.CreateAsync(product);
            await publishEndpoint.Publish(new ProductItemCreated(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity
               
             ));

             return _mapper.Map<ProductDto>(product);      

        }
    }
}
