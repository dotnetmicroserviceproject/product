using AutoMapper;
using common.MongoDB.Interface;
using MediatR;
using Product.Service.Entities;
using Product.Service.Features.Products.Dtos;
using Product.Service.Features.Products.Queries;



namespace Product.Service.Features.Products.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {

        private readonly IMongoRepository<Items> _productRepository;
        private readonly IMapper _mapper;
        public GetProductByIdQueryHandler(IMapper mapper, IMongoRepository<Items> productRepository)
        {

            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return null;
            }

            var product = await _productRepository.GetAsync(request.Id);
            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(product);
        }
    }
}
