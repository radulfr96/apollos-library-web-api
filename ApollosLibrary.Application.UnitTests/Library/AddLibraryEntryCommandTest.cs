using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.UnitTests.Library
{
    [Collection("UnitTestCollection")]
    public class AddLibraryEntryCommandTest : TestBase
    {
        private readonly AddLibraryEntryCommandValidator _validator;
        private readonly Faker _faker;

        public AddLibraryEntryCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddLibraryEntryCommandValidator();
            _faker = new Faker();
        }

        [Fact]
        public void BookIdInvalidValue()
        {
            var command = new AddLibraryEntryCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BookId);
        }

        [Fact]
        public void LibraryIdInvalidValue()
        {
            var command = new AddLibraryEntryCommand()
            {
                BookId = _faker.Random.Int(1),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.LibraryId);
        }

        [Fact]
        public void QuantityInvalidValue()
        {
            var command = new AddLibraryEntryCommand()
            {
                BookId = _faker.Random.Int(1),
                LibraryId = _faker.Random.Int(1),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Quantity);
        }

        [Fact]
        public async Task LibraryNotFoundException()
        {
            var userId = Guid.NewGuid();
            var command = new AddLibraryEntryCommand()
            {
                BookId = 1,
                LibraryId = 1,
                Quantity = 1,
            };

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            var bookDataLayer = new Mock<IBookDataLayer>();
            bookUnitOfWork.Setup(s => s.BookDataLayer).Returns(bookDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var libraryUnitOfWork = new Mock<ILibraryUnitOfWork>();
            var libraryDataLayer = new Mock<ILibraryDataLayer>();
            libraryUnitOfWork.Setup(s => s.LibraryDataLayer).Returns(libraryDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return libraryUnitOfWork.Object;
            });

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(u => u.GetUserId()).Returns(userId);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<LibraryNotFoundException>();
        }

        [Fact]
        public async Task UserCannotEditLibraryException()
        {
            var userId = Guid.NewGuid();
            var command = new AddLibraryEntryCommand()
            {
                BookId = 1,
                LibraryId = 1,
                Quantity = 1,
            };

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            var bookDataLayer = new Mock<IBookDataLayer>();
            bookUnitOfWork.Setup(s => s.BookDataLayer).Returns(bookDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var libraryUnitOfWork = new Mock<ILibraryUnitOfWork>();
            var libraryDataLayer = new Mock<ILibraryDataLayer>();
            libraryDataLayer.Setup(d => d.GetLibrary(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Library()
            {
                LibraryId = 1,
                UserId = Guid.NewGuid(),
            }));
            libraryUnitOfWork.Setup(s => s.LibraryDataLayer).Returns(libraryDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return libraryUnitOfWork.Object;
            });

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(u => u.GetUserId()).Returns(userId);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<UserCannotModifyLibraryException>();
        }

        [Fact]
        public async Task BookNotFoundException()
        {
            var userId = Guid.NewGuid();
            var command = new AddLibraryEntryCommand()
            {
                BookId = 1,
                LibraryId = 1,
                Quantity = 1,
            };

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            var bookDataLayer = new Mock<IBookDataLayer>();
            bookUnitOfWork.Setup(s => s.BookDataLayer).Returns(bookDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var libraryUnitOfWork = new Mock<ILibraryUnitOfWork>();
            var libraryDataLayer = new Mock<ILibraryDataLayer>();
            libraryUnitOfWork.Setup(s => s.LibraryDataLayer).Returns(libraryDataLayer.Object);
            libraryDataLayer.Setup(d => d.GetLibrary(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Library()
            {
                LibraryId = 1,
                UserId = userId,
            }));
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return libraryUnitOfWork.Object;
            });

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(u => u.GetUserId()).Returns(userId);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BookNotFoundException>();
        }
    }
}
