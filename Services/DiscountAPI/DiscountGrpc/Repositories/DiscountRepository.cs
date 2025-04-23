using Discount.Grpc.Entities;
using Npgsql;
using Dapper;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        #region Constructor
        private readonly string _connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }

        #endregion


        #region Connection Methods for fast coding
        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
        #endregion

        #region Get Coupon

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = CreateConnection();

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                 ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
            {
                return new Coupon()
                {
                    Amount = 0,
                    Description = "there is no discount",
                    ProductName = "no discount"
                };
            }

            return coupon;
        }

        #endregion


        #region Create Coupon

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = CreateConnection();

            var affected = await connection.ExecuteAsync
                  ("INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)",
                  new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected == 0 ? false : true;
        }

        #endregion

        #region Update Coupon

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = CreateConnection();

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Amount WHERE id=@CouponId",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount ,CouponId = coupon.Id});

            return affected == 0 ? false : true;
        }

        #endregion

        #region Delete Coupon

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = CreateConnection();

            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName=@ProductName",
                new { ProductName = productName });

            return affected == 0 ? false : true;
        }

        #endregion
    }
}
