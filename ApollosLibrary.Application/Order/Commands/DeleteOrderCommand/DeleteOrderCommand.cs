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

namespace ApollosLibrary.Application.Order.Commands.DeleteOrderCommand
{
    public class DeleteOrderCommand : IRequest<DeleteOrderCommandDto>
    {
        public int OrderId { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderCommandDto>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IUserService _userService;

        public DeleteOrderCommandHandler(
            IOrderUnitOfWork orderUnitOfWork
            , IUserService userService)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _userService = userService;
        }

        public async Task<DeleteOrderCommandDto> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _orderUnitOfWork.OrderDataLayer.GetOrder(command.OrderId);

            if (order == null)
            {
                throw new OrderNotFoundException($"Unable to find order with id of [{command.OrderId}]");
            }
            else if (order.UserId != _userService.GetUserId())
            {
                throw new UserCannotModifyOrderException($"User is unauthourized to modify order [{command.OrderId}]");
            }

            _orderUnitOfWork.OrderDataLayer.DeleteOrder(order);

            await _orderUnitOfWork.Save();

            return new DeleteOrderCommandDto();
        }
    }
}
