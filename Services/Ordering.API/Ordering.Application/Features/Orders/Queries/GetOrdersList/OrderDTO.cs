using Ordering.Domain.Entities.Address;
using Ordering.Domain.Entities.Order;
using Ordering.Domain.Entities.Payment;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class OrderDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public HashSet<OrderAddressDTO> Addresses { get; set; }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string CardName { get; set; }
    public string RefCode { get; set; }
    PaymentMethod PaymentMethod { get; set; }
}