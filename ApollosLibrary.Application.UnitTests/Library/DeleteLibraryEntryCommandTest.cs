using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Commands.DeleteLibraryEntryCommand;
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
    public class DeleteLibraryEntryCommandTest : TestBase
    {
        private readonly DeleteLibraryEntryCommandValidator _validator;
        private readonly Faker _faker;

        public DeleteLibraryEntryCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new DeleteLibraryEntryCommandValidator();
            _faker = new Faker();
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new DeleteLibraryEntryCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.LibraryEntryId);
        }

        [Fact]
        public async Task LibraryNotFoundException()
        {
            var userId = Guid.NewGuid();
            var command = new DeleteLibraryEntryCommand()
            {
                LibraryEntryId = 1,
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
        public async Task LibraryEntryNotFoundException()
        {
            var userId = Guid.NewGuid();
            var command = new DeleteLibraryEntryCommand()
            {
                LibraryEntryId = 1,
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
            libraryDataLayer.Setup(l => l.GetLibraryIdByUserId(userId)).Returns(Task.FromResult((int?)1));
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
            await act.Should().ThrowAsync<LibraryEntryNotFoundException>();
        }

        [Fact]
        public async Task UserCannotEditLibraryException()
        {
            var userId = Guid.NewGuid();
            var command = new DeleteLibraryEntryCommand()
            {
                LibraryEntryId = 1,
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
            libraryDataLayer.Setup(l => l.GetLibraryIdByUserId(userId)).Returns(Task.FromResult((int?)2));
            libraryDataLayer.Setup(d => d.GetLibraryEntry(It.IsAny<int>())).Returns(Task.FromResult(new Domain.LibraryEntry()
            {
                LibraryId = 1,
                BookId = 1,
                EntryId = 1,
                Quantity = 1,
                Library = new Domain.Library()
                {
                    LibraryId = 1,
                    UserId = Guid.NewGuid(),
                }
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
    }
}
