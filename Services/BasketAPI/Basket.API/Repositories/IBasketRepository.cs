using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetUserBasket(string userName);

        //Update and Insert is the same in this.
        //cause if that object does'nt exsists, will add to redis .
        //if is exists that object will update.
        Task<ShoppingCart> UpdateUserBasket(ShoppingCart basket);
        Task DeleteUserBasket(string userName);
    }
}
