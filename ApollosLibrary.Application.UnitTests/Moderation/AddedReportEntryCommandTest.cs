using ApollosLibrary.Application.Moderation.Commands.AddReportEntryCommand;
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
    public class AddedReportEntryCommandTest : TestBase
    {
        private readonly AddReportEntryCommandValidator _validator;

        public AddedReportEntryCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddReportEntryCommandValidator();
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new AddReportEntryCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.EntryId);
        }
    }
}
