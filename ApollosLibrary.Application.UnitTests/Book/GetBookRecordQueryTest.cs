using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentValidation.TestHelper;

using FluentAssertions;
using ApollosLibrary.Application.Book.Queries.GetBookRecordQuery;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class GetBookRecordQueryTest : TestBase
    {
        private readonly GetBookRecordQueryValidator _validator;

        public GetBookRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetBookRecordQueryValidator();
        }

        [Fact]
        public void BookIdInvalidValue()
        {
            var query = new GetBookRecordQuery()
            {
                BookRecordId = 0,
            };

            var result = _validator.TestValidate(query);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BookRecordId);
        }

        [Fact]
        public async Task BookRecordNotFound()
        {
            var command = new GetBookRecordQuery()
            {
                BookRecordId = 1,
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetBookByISBN(It.IsAny<string>())).Returns(Task.FromResult(new Domain.Book()));

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();
            genreUnitOfWork.Setup(r => r.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BookRecordNotFoundException>();
        }
    }
}
