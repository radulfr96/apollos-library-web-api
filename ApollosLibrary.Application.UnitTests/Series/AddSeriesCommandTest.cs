using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Series.Commands.AddSeriesCommand;
using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
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

        public AddSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddSeriesCommandValidator();
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
            var faker = new Faker();

            var command = new AddSeriesCommand()
            {
                Name = faker.Random.AlphaNumeric(10),
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
    }
}
