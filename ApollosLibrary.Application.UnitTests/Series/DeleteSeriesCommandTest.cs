
using ApollosLibrary.Application.Series.Commands.DeleteSeriesCommand;
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
    public class DeleteSeriesCommandTest : TestBase
    {
        private readonly DeleteSeriesCommandValidator _validator;

        public DeleteSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new DeleteSeriesCommandValidator();
        }

        [Fact]
        public void SeriesIdInvalidValue()
        {
            var command = new DeleteSeriesCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.SeriesId);
        }
    }
}
