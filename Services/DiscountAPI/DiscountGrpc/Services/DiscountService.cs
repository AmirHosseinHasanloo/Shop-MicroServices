using System.Runtime.CompilerServices;
using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        #region Constractor

        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountRepository> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountRepository> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region Get Discount

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"There is no discount with {request.ProductName} product name!"));
            }

            _logger.LogInformation("Discount is retrived for product name !");

            return _mapper.Map<CouponModel>(coupon);
        }

        #endregion
    }
}
