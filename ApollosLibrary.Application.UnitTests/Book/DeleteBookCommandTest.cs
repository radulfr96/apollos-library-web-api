using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Genre.Commands.DeleteGenreCommand;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Application.Book.Commands.DeleteBookCommand;
using FluentValidation.TestHelper;
using FluentAssertions;


namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class DeleteBookCommandTest : TestBase
    {
        private readonly DeleteBookCommandValidator _validator;

        public DeleteBookCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new DeleteBookCommandValidator();
        }

        [Fact]
        public void BookIdInvalidValue()
        {
            var command = new DeleteBookCommand()
            {
                BookId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BookId);
        }

        [Fact]
        public async Task BookNotFound()
        {
            var command = new DeleteBookCommand()
            {
                BookId = 1,
            };

            var mockUserService = new Mock<IUserService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var mockDateTimeService = new Mock<IDateTimeService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookUnitOfWork.Setup(s => s.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<BookNotFoundException>(() => mediator.Send(command));
        }
    }
}
