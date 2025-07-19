namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class OrderAddressDTO
{
    public string UserAddress { get; set; }
    public int OrderId { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
}