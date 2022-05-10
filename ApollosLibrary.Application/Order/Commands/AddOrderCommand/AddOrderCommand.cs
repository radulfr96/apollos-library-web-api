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
        private readonly IUserService _userService;

        public AddOrderCommandHandler(IOrderUnitOfWork orderUnitOfWork, IUserService userService, IBookUnitOfWork bookUnitOfWork)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _userService = userService;
            _bookUnitOfWork = bookUnitOfWork;
        }

        public async Task<AddOrderCommandDto> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var response = new AddOrderCommandDto();

            

            return response;
        }
    }
}
