using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order;
using ApollosLibrary.Application.Order.Commands.AddOrderCommand;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.UnitTests.Order
{
    [Collection("UnitTestCollection")]
    public class AddOrderCommandTest : TestBase
    {
        private readonly AddOrderCommandValidator _validator;
        private readonly IDateTimeService _dateTimeService;

        public AddOrderCommandTest(TestFixture fixture) : base(fixture)
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(s => s.Now).Returns(new DateTime(2022, 05, 11, 21, 32, 00));
            _dateTimeService = dateTimeService.Object;

            _validator = new AddOrderCommandValidator(_dateTimeService);
        }

        [Fact]
        public void BusinessIdInvalidValue()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BusinessId);
        }

        [Fact]
        public void OrderDateIsInFutureIdInvalidValue()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(1),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.OrderDate);
        }

        [Fact]
        public void OrderHasNoItems()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.OrderItems);
        }

        [Fact]
        public void OrderItemInvalidBookId()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 0,
                    }
                }
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("OrderItems[0].BookId");
        }

        [Fact]
        public void OrderItemInvalidPrice()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        UnitPrice = -1.00m,
                    }
                }
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("OrderItems[0].UnitPrice");
        }

        [Fact]
        public void OrderItemInvalidQuantity()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        UnitPrice = 15.00m,
                        Quantity = 0,
                    }
                }
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("OrderItems[0].Quantity");
        }

        [Fact]
        public async Task OrderItemBusinessIsNotBookshop()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        UnitPrice = 15.00m,
                        Quantity = 1,
                    }
                }
            };

            var bookDataLayer = new Mock<IBookDataLayer>();

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var businessDataLayer = new Mock<IBusinessDataLayer>();
            businessDataLayer.Setup(b => b.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()
            {
                BusinessId = 1,
                BusinessTypeId = (int)BusinessTypeEnum.Publisher,
            }));

            var businessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            businessUnitOfWork.Setup(b => b.BusinessDataLayer).Returns(businessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return businessUnitOfWork.Object;
            });

            var orderDataLayer = new Mock<IOrderDataLayer>();

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(b => b.OrderDataLayer).Returns(orderDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return orderUnitOfWork.Object;
            });

            var userService = new Mock<IUserService>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return userService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BusinessIsNotBookshopException>();
        }

        [Fact]
        public async Task OrderItemBookItemNotFound()
        {
            var command = new AddOrderCommand()
            {
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        UnitPrice = 15.00m,
                        Quantity = 1,
                    }
                }
            };

            var bookDataLayer = new Mock<IBookDataLayer>();

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var businessDataLayer = new Mock<IBusinessDataLayer>();
            businessDataLayer.Setup(b => b.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()
            {
                BusinessId = 1,
                BusinessTypeId = (int)BusinessTypeEnum.Bookshop,
            }));

            var businessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            businessUnitOfWork.Setup(b => b.BusinessDataLayer).Returns(businessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return businessUnitOfWork.Object;
            });

            var orderDataLayer = new Mock<IOrderDataLayer>();

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(b => b.OrderDataLayer).Returns(orderDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return orderUnitOfWork.Object;
            });

            var userService = new Mock<IUserService>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return userService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BookNotFoundException>();
        }
    }
}
