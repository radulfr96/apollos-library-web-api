using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order.Commands.DeleteOrderCommand;
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
    public class DeleteOrderCommandTest : TestBase
    {
        private readonly DeleteOrderCommandValidator _validator;
        private readonly IDateTimeService _dateTimeService;

        public DeleteOrderCommandTest(TestFixture fixture) : base(fixture)
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(s => s.Now).Returns(new DateTime(2022, 05, 11, 21, 32, 00));
            _dateTimeService = dateTimeService.Object;

            _validator = new DeleteOrderCommandValidator();
        }

        [Fact]
        public void OrderIdInvalidValue()
        {
            var command = new DeleteOrderCommand()
            {
                OrderId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.OrderId);
        }

        [Fact]
        public async Task OrderIsNotFound()
        {
            var command = new DeleteOrderCommand()
            {
                OrderId = 1,
            };

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

            var command = new DeleteOrderCommand()
            {
                OrderId = 1,
            };

            var orderDataLayer = new Mock<IOrderDataLayer>();
            orderDataLayer.Setup(o => o.GetOrder(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Order()
            {
                OrderId = command.OrderId,
                BusinessId = 1,
                OrderDate = _dateTimeService.Now,
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
            await act.Should().ThrowAsync<UserCannotModifyOrderException>();
        }
    }
}
