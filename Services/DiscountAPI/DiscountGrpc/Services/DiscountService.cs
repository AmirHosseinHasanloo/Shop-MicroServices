using System.Runtime.CompilerServices;
using AutoMapper;
using Discount.Grpc.Entities;
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

        #region Create Discount

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            if (request.Coupon == null || string.IsNullOrWhiteSpace(request.Coupon.ProductName))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Valid coupon data is required!"));
            }
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var IsCreated = await _discountRepository.CreateDiscount(coupon);

            if (!IsCreated)
            {
                throw new RpcException(new Status(StatusCode.NotFound,$"Discount for '{request.Coupon.ProductName}' not found!"));
            }

            _logger.LogInformation("the Discount is created up");
            return _mapper.Map<CouponModel>(coupon);
        }

        #endregion

        #region Update Discount

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            if (request.Coupon == null || string.IsNullOrWhiteSpace(request.Coupon.ProductName))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Valid coupon data is required."));

            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var isUpdated = await _discountRepository.UpdateDiscount(coupon);

            if (!isUpdated)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for '{request.Coupon.ProductName}' not found."));

            _logger.LogInformation("the Discount is updated up");
            return _mapper.Map<CouponModel>(coupon);
        }


        #endregion

        #region Delete Discount

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Product name is required."));

            var isDeleted = await _discountRepository.DeleteDiscount(request.ProductName);

            if (!isDeleted)
                throw new RpcException(new Status(StatusCode.NotFound, $"No discount found for '{request.ProductName}'."));

            _logger.LogInformation("the Discount is deleted up");
            return new DeleteDiscountResponse { IsDeleted = true };
        }


        #endregion
    }
}
