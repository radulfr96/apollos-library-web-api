using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order;
using ApollosLibrary.Application.Order.Commands.AddOrderCommand;
using ApollosLibrary.Application.Order.Commands.UpdateOrderCommand;
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
    public class UpdateOrderCommandTest : TestBase
    {
        private readonly UpdateOrderCommandValidator _validator;
        private readonly IDateTimeService _dateTimeService;

        public UpdateOrderCommandTest(TestFixture fixture) : base(fixture)
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(s => s.Now).Returns(new DateTime(2022, 05, 11, 21, 32, 00));
            _dateTimeService = dateTimeService.Object;

            _validator = new UpdateOrderCommandValidator(_dateTimeService);
        }

        [Fact]
        public void OrderIdInvalidValue()
        {
            var command = new UpdateOrderCommand()
            {
                OrderId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.OrderId);
        }

        [Fact]
        public void BusinessIdInvalidValue()
        {
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BusinessId);
        }

        [Fact]
        public void OrderDateIsInFutureIdInvalidValue()
        {
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
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
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
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
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
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
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        Price = -1.00m,
                    }
                }
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("OrderItems[0].Price");
        }

        [Fact]
        public void OrderItemInvalidQuantity()
        {
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        Price = 15.00m,
                        Quantity = 0,
                    }
                }
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("OrderItems[0].Quantity");
        }

        [Fact]
        public async Task OrderIsNotFound()
        {
            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        Price = 15.00m,
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
            await act.Should().ThrowAsync<OrderNotFoundException>();
        }

        [Fact]
        public async Task UserCannotModifyOrder()
        {
            var userId = Guid.NewGuid();

            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        Price = 15.00m,
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

            var businessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            businessUnitOfWork.Setup(b => b.BusinessDataLayer).Returns(businessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return businessUnitOfWork.Object;
            });

            var orderDataLayer = new Mock<IOrderDataLayer>();
            orderDataLayer.Setup(o => o.GetOrder(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Order()
            {
                OrderId = command.OrderId,
                BusinessId = command.BusinessId,
                OrderDate = command.OrderDate,
                UserId = userId,
            }));

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(b => b.OrderDataLayer).Returns(orderDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return orderUnitOfWork.Object;
            });

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserId()).Returns(Guid.NewGuid());

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return userService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<UserCannotAccessOrderException>();
        }

        [Fact]
        public async Task OrderItemBusinessIsNotBookshop()
        {
            var userID = Guid.NewGuid();

            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        Price = 15.00m,
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
            orderDataLayer.Setup(o => o.GetOrder(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Order()
            {
                OrderId = command.OrderId,
                BusinessId = command.BusinessId,
                OrderDate = command.OrderDate,
                UserId = userID,
            }));

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(b => b.OrderDataLayer).Returns(orderDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return orderUnitOfWork.Object;
            });

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserId()).Returns(userID);

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
        public async Task OrderItemBookNotFound()
        {
            var userId = Guid.NewGuid();

            var command = new UpdateOrderCommand()
            {
                OrderId = 1,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now.AddDays(-2),
                OrderItems = new List<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        BookId = 1,
                        Price = 15.00m,
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
            orderDataLayer.Setup(o => o.GetOrder(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Order()
            {
                OrderId = command.OrderId,
                BusinessId = command.BusinessId,
                OrderDate = command.OrderDate,
                UserId = userId,
            }));

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(b => b.OrderDataLayer).Returns(orderDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return orderUnitOfWork.Object;
            });

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserId()).Returns(userId);

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
