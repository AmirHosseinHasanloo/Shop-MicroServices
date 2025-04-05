namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public int TotalPrice
        {
            get
            {
                int totalPrice = 0;
                foreach (var item in ShoppingCartItems)
                {
                    totalPrice += item.Price * item.Count;
                }
                return totalPrice;
            }
        }
    }
}
