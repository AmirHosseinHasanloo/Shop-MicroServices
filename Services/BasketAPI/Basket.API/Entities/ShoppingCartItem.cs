namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
    }
}
