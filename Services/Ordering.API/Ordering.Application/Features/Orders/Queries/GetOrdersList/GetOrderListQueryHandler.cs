using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrderListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderDTO>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderDTO>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
       var OrdersListByName = await _orderRepository.GetOrdersByUserName(request.UserName);

       return _mapper.Map<List<OrderDTO>>(OrdersListByName); 
    }
}