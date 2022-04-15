using ApollosLibrary.Application.Author.Commands.UpdateAuthorCommand;
using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Series.Commands.UpdateSeriesCommand;
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
    public class UpdateSeriesCommandTest : TestBase
    {
        private readonly UpdateSeriesCommandValidator _validator;

        public UpdateSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new UpdateSeriesCommandValidator();
        }

        [Fact]
        public void SeriesIdInvalidValue()
        {
            var command = new UpdateSeriesCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.SeriesIdInvalidValue.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void SeriesNameNotProvided()
        {
            var command = new UpdateSeriesCommand();

            var result = _validator.TestValidate(command);


            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.SeriesNameNotProvided.ToString()).Any().Should().BeTrue();

            command.Name = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.SeriesNameNotProvided.ToString()).Any().Should().BeTrue();
        }
    }
}
