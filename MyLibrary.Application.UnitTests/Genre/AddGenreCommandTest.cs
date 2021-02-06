using Bogus;
using FluentValidation.TestHelper;
using MyLibrary.Application.Common.Enums;
using MyLibrary.Application.Genre.Commands.AddGenreCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.UnitTests
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

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.GenreNameNotProvided.ToString()).Any());

            command.Name = "";

            result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.GenreNameNotProvided.ToString()).Any());
        }

        [Fact]
        public void GenreNameInvalidLength()
        {
            var command = new AddGenreCommand()
            {
                Name = new Faker().Lorem.Random.String(51)
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.GenreNameInvalidLength.ToString()).Any());
        }
    }
}
