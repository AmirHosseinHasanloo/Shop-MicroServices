using MediatR;
using Ordering.Domain.Entities.Address;
using Ordering.Domain.Entities.Payment;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommand : IRequest<int>
{
    public string UserName { get; set; }

    public ICollection<Address> Addresses { get; set; }

    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string CardName { get; set; }
    public string RefCode { get; set; }
    PaymentMethod PaymentMethod { get; set; }
}