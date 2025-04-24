using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        #region Constractor

        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient;
        }

        #endregion

        #region Get Discount

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var GetDiscountRequest = new GetDiscountRequest { ProductName = productName };

            return await _discountProtoServiceClient.GetDiscountAsync(GetDiscountRequest);
        }

        #endregion
    }
}
