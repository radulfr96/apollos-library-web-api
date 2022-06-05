using Bogus;
using FluentValidation.TestHelper;

using ApollosLibrary.Application.Genre.Commands.AddGenreCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class AddGenreCommandTest : TestBase
    {
        private readonly AddGenreCommandValidator _validator;

        public AddGenreCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddGenreCommandValidator();
        }

        [Fact]
        public void GenreNameNotProvided()
        {
            var command = new AddGenreCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Name);

            command.Name = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Name);
        }

        [Fact]
        public void GenreNameInvalidLength()
        {
            var command = new AddGenreCommand()
            {
                Name = new Faker().Lorem.Random.String(51)
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Name);
        }
    }
}
