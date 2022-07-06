using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain.Enums;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Order.Commands.UpdateOrderCommand
{
    public class UpdateOrderCommand : IRequest<UpdateOrderCommandDto>
    {
        public int OrderId { get; set; }
        public int BusinessId { get; set; }
        public LocalDateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderCommandDto>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IBusinessUnitOfWork _businessUnitOfWork;
        private readonly IUserService _userService;

        public UpdateOrderCommandHandler(
            IOrderUnitOfWork orderUnitOfWork
            , IBookUnitOfWork bookUnitOfWork
            , IBusinessUnitOfWork businessUnitOfWork
            , IUserService userService)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _bookUnitOfWork = bookUnitOfWork;
            _businessUnitOfWork = businessUnitOfWork;
            _userService = userService;
        }

        public async Task<UpdateOrderCommandDto> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _orderUnitOfWork.OrderDataLayer.GetOrder(command.OrderId);

            if (order == null)
            {
                throw new OrderNotFoundException($"Unable to find order with id of [{command.OrderId}]");
            }
            else if (order.UserId != _userService.GetUserId())
            {
                throw new UserCannotAccessOrderException($"You do not have permission to modify order with id [{command.OrderId}]");
            }

            var business = await _businessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId);

            if (business == null)
            {
                throw new BusinessNotFoundException($"Unable to find publisher with business id [{command.BusinessId}]");
            }
            else if (business.BusinessTypeId != (int)BusinessTypeEnum.Bookshop)
            {
                throw new BusinessIsNotBookshopException($"Business with with id of [{command.BusinessId}] is not a bookshop");
            }

            foreach (var item in command.OrderItems)
            {
                var book = await _bookUnitOfWork.BookDataLayer.GetBook(item.BookId);

                if (book == null)
                {
                    throw new BookNotFoundException($"Unable to find book with id of [{item.BookId}]");
                }
            }

            order.OrderDate = command.OrderDate;
            order.BusinessId = command.BusinessId;
            order.OrderDate = command.OrderDate;

            await _orderUnitOfWork.OrderDataLayer.DeleteOrderItems(command.OrderId);

            order.OrderItems = command.OrderItems.Select(o => new Domain.OrderItem()
            {
                BookId = o.BookId,
                Price = o.UnitPrice,
                Quantity = o.Quantity,
            }).ToList();

            await _orderUnitOfWork.Save();

            return new UpdateOrderCommandDto();
        }
    }
}
