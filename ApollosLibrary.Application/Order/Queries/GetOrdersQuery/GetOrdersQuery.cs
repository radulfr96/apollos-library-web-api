using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Queries.GetOrdersQuery
{
    public class GetOrdersQuery : IRequest<GetOrdersQueryDto>
    {
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, GetOrdersQueryDto>
    {
        private readonly IUserService _userService;
        private readonly IOrderUnitOfWork _orderUnitOfWork;

        public GetOrdersQueryHandler(IUserService userService, IOrderUnitOfWork orderUnitOfWork)
        {
            _userService = userService;
            _orderUnitOfWork = orderUnitOfWork;
        }

        public async Task<GetOrdersQueryDto> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var orders = await _orderUnitOfWork.OrderDataLayer.GetOrders(_userService.GetUserId());

            var response = new GetOrdersQueryDto()
            {
                Orders = orders.Select(o => new OrderDTO()
                {
                    Bookshop = o.Business.Name,
                    NumberOfItems = o.OrderItems.Count(),
                    OrderId = o.OrderId,
                    Total = GetOrderTotal(o.OrderItems),
                }).ToList(),
            };

            return response;
        }

        private decimal GetOrderTotal(List<OrderItem> orderItems)
        {
            decimal total = 0.00m;

            foreach (var item in orderItems)
            {
                total += item.Price * item.Quantity;
            }

            return total;
        }
    }
}
