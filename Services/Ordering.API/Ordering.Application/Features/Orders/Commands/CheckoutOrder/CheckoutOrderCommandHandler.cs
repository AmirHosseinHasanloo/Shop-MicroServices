using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities.Order;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository,
        IMapper mapper, IEmailService emailService,
        ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        var newOrder = await _orderRepository.AddAsync(orderEntity);

        _logger.LogInformation($"Order with id : {newOrder.Id} is successfully created.");
        //TODO : send Email
        return newOrder.Id;
    }

    public async Task SendEmail(Order order)
    {
        try
        {
            await _emailService.SendEmailAsync(new Email()
            {
                To = order.EmailAddress,
                Subject = $"new order has been created",
                Body = "order has been created"
            });
        }
        catch (Exception e)
        {
          _logger.LogError("email has not been send ");
        }
    }
}