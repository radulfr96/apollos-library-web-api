using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Genre.Queries.GetGenreQuery;
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

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class GetGenreQueryTest : TestBase
    {
        private readonly GetGenreQueryValidator _validator;

        public GetGenreQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetGenreQueryValidator();
        }

        [Fact]
        public void GenreIdInvalidValue()
        {
            var command = new GetGenreQuery()
            {
                GenreId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.GenreId);
        }

        [Fact]
        public async Task GenreNotFound()
        {
            var command = new GetGenreQuery()
            {
                GenreId = 1,
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

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();
            genreUnitOfWork.Setup(s => s.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<GenreNotFoundException>(() => mediator.Send(command));
        }
    }
}
