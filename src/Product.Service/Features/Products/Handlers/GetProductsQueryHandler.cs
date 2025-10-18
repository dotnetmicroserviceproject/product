using AutoMapper;
using common.MongoDB.Interface;
using MediatR;
using Product.Service.Entities;
using Product.Service.Features.Products.Dtos;
using Product.Service.Features.Products.Queries;


namespace Product.Service.Features.Products.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IMongoRepository<Items> _productRepository;
        private readonly IMapper _mapper;
        public GetProductsQueryHandler(IMapper mapper, IMongoRepository<Items> productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);


            return mappedProducts;
        }
    }
}
