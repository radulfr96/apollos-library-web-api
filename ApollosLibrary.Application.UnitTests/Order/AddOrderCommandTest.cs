using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Order.Commands.AddOrderCommand;
using FluentAssertions;
using FluentValidation.TestHelper;
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
                OrderItems = new List<AddOrderCommand.OrderItemDTO>()
                {
                    new AddOrderCommand.OrderItemDTO()
                    {
                        BookId = 0,
                    }
                }
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("OrderItems[0].BookId");
        }
    }
}
