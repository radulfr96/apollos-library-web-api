using ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.UnitTests.Moderation
{
    [Collection("UnitTestCollection")]
    public class GetReportEntryQueryTest : TestBase
    {
        private readonly GetEntryReportQueryValidator _validator;

        public GetReportEntryQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetEntryReportQueryValidator();
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new GetEntryReportQuery();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.ReportEntryId);
        }
    }
}
