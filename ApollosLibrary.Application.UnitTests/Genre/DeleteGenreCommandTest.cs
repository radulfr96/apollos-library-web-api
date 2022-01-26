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

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class DeleteGenreCommandTest : TestBase
    {
        public DeleteGenreCommandTest(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GenreNotFound()
        {
            var command = new DeleteGenreCommand()
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
