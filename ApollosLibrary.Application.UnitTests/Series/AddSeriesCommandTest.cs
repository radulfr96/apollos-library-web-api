using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Series.Commands.AddSeriesCommand;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using Bogus;
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

namespace ApollosLibrary.Application.UnitTests.Series
{
    [Collection("UnitTestCollection")]
    public class AddSeriesCommandTest : TestBase
    {
        private readonly AddSeriesCommandValidator _validator;
        private readonly Faker _faker;

        public AddSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new();
            _faker = new();
        }

        [Fact]
        public void SeriesNameNotProvided()
        {
            var command = new AddSeriesCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.SeriesNameNotProvided.ToString()).Any().Should().BeTrue();

            command.Name = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.SeriesNameNotProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void BookIdInvalid()
        {
            var command = new AddSeriesCommand()
            {
                Name = _faker.Random.AlphaNumeric(10),
                SeriesOrder = new Dictionary<int, int>(),
            };

            command.SeriesOrder.Add(0, 1);

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.BookIdInvalidValue.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void OrderInvalidValue()
        {
            var faker = new Faker();

            var command = new AddSeriesCommand()
            {
                Name = faker.Random.AlphaNumeric(10),
                SeriesOrder = new Dictionary<int, int>(),
            };

            command.SeriesOrder.Add(1, 0);

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.BookOrderInvalidValue.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public async Task BookIdNotFoundError()
        {
            var command = new AddSeriesCommand()
            {
                Name = _faker.Random.AlphaNumeric(10),
                SeriesOrder = new Dictionary<int, int>()
            };

            command.SeriesOrder.Add(1, 2);

            var mockDataLayer = new Mock<ISeriesDataLayer>();
            var mockSeriesUow = new Mock<ISeriesUnitOfWork>();
            mockSeriesUow.Setup(s => s.SeriesDataLayer).Returns(mockDataLayer.Object);

            var mockBookDataLayer = new Mock<IBookDataLayer>();
            var mockBookSeriesUow = new Mock<IBookUnitOfWork>();
            mockBookSeriesUow.Setup(s => s.BookDataLayer).Returns(mockBookDataLayer.Object);

            var mockUserService = new Mock<IUserService>();

            Mock<IDateTimeService> mockDateTimeService = new();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockBookSeriesUow.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockSeriesUow.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BookNotFoundException>();
        }
    }
}
