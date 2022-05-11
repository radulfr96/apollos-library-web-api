using ApollosLibrary.Application.Common.Enums;
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

namespace ApollosLibrary.Application.Order.Commands.AddOrderCommand
{
    public class AddOrderCommand : IRequest<AddOrderCommandDto>
    {
        public int BusinessId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();

        public class OrderItemDTO
        {
            public int BookId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }

    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, AddOrderCommandDto>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IBusinessUnitOfWork _businessUnitOfWork;
        private readonly IUserService _userService;

        public AddOrderCommandHandler(
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

        public async Task<AddOrderCommandDto> Handle(AddOrderCommand command, CancellationToken cancellationToken)
        {
            var business = await _businessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId);

            if (business == null)
            {
                throw new BusinessNotFoundException($"Unable to find publisher with business id [{command.BusinessId}]");
            }
            else if (business.BusinessTypeId == (int)BusinessTypeEnum.Bookshop)
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

            var order = new Domain.Order()
            {
                BusinessId = command.BusinessId,
                OrderDate = command.OrderDate,
                UserId = _userService.GetUserId(),
                OrderItems = command.OrderItems.Select(i => new Domain.OrderItem()
                {
                    BookId = i.BookId,
                    Price = i.Price,
                    Quantity = i.Quantity,
                }).ToList(),
            };

            await _orderUnitOfWork.OrderDataLayer.AddOrder(order);
            await _orderUnitOfWork.Save();

            return new AddOrderCommandDto()
            {
                OrderId = order.OrderId,
            };
        }
    }
}
