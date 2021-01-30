using Bogus;
using MyLibrary.Application.Book.Commands.AddBookCommand;
using MyLibrary.Application.XUnitTestProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class AddBookCommandTest : TestBase
    {
        private readonly AddBookCommandValidator _validatior;
        private readonly Faker _faker;

        public AddBookCommandTest(TestFixture fixture) : base(fixture)
        {
            _validatior = new AddBookCommandValidator();
            _faker = new Faker();
        }


    }
}
