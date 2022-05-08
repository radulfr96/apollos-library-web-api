
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Queries.GetLibraryEntryQuery;
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
    public class GetLibraryEntryQueryTest : TestBase
    {
        private readonly GetLibraryEntryQueryValidator _validator;
        private readonly Faker _faker;

        public GetLibraryEntryQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetLibraryEntryQueryValidator();
            _faker = new Faker();
            _fixture = fixture;
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new GetLibraryEntryQuery();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.EntryId);
        }

        [Fact]
        public async Task LibraryNotFoundException()
        {
            var userId = Guid.NewGuid();
            var command = new GetLibraryEntryQuery()
            {
                EntryId = 1,
            };

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
            await act.Should().ThrowAsync<LibraryEntryNotFoundException>();
        }

        [Fact]
        public async Task UserUnauthorizedException()
        {
            var userId = Guid.NewGuid();
            var command = new GetLibraryEntryQuery()
            {
                EntryId = 1,
            };

            var libraryUnitOfWork = new Mock<ILibraryUnitOfWork>();
            var libraryDataLayer = new Mock<ILibraryDataLayer>();
            libraryDataLayer.Setup(l => l.GetLibraryIdByUserId(It.IsAny<Guid>())).Returns(Task.FromResult((int?)2));
            libraryDataLayer.Setup(l => l.GetLibraryEntry(It.IsAny<int>())).Returns(Task.FromResult(new Domain.LibraryEntry()
            {
                BookId = 1,
                EntryId = 1,
                LibraryId = 1,
                Quantity = 1,
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