using ApollosLibrary.Application.Moderation.Commands.UpdateEntryReportCommand;
using Bogus;
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
    public class UpdateEntryReportCommandTest : TestBase
    {
        private readonly UpdateEntryReportCommandValidator _validator;

        public UpdateEntryReportCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new UpdateEntryReportCommandValidator();
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new UpdateEntryReportCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.EntryReportId);
        }

        [Fact]
        public void EntryStatusInvalidValue()
        {
            var command = new UpdateEntryReportCommand()
            {
                EntryReportId = new Faker().Random.Int(1),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.EntryReportStatus);
        }
    }
}
