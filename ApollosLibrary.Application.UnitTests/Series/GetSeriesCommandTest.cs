using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Series.Queries.GetSeriesQuery;
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
    public class GetSeriesCommandTest : TestBase
    {
        private readonly GetSeriesQueryValidator _validator;

        public GetSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetSeriesQueryValidator();
        }

        [Fact]
        public void SeriesIdInvalidValue()
        {
            var command = new GetSeriesQuery();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.SeriesIdInvalidValue.ToString()).Any().Should().BeTrue();
        }
    }
}
