using ApollosLibrary.Application.Moderation.Commands.AddEntryReportCommand;
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
    public class AddedEntryReportCommandTest : TestBase
    {
        private readonly AddEntryReportCommandValidator _validator;

        public AddedEntryReportCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddEntryReportCommandValidator();
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new AddEntryReportCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.EntryId);
        }
    }
}
