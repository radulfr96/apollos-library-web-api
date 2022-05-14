using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Queries.GetOrderQuery
{
    public class GetOrderQuery : IRequest<GetOrderQueryDto>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, GetOrderQueryDto>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IUserService _userService;

        public GetOrderQueryHandler(
            IOrderUnitOfWork orderUnitOfWork
            , IUserService userService)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _userService = userService;
        }

        public async Task<GetOrderQueryDto> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            var order = await _orderUnitOfWork.OrderDataLayer.GetOrder(query.OrderId);

            if (order == null)
            {
                throw new OrderNotFoundException($"Unable to find order with id of [{query.OrderId}]");
            }
            else if (order.UserId != _userService.GetUserId())
            {
                throw new UserCannotAccessOrderException($"User cannot access order with id of [{query.OrderId}]");
            }

            return new GetOrderQueryDto()
            {
                BusinessId = order.BusinessId,
                OrderDate = order.OrderDate,
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(i => new OrderItemDTO()
                {
                    BookId = i.BookId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
            };
        }
    }
}
