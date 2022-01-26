using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Genre.Commands.DeleteGenreCommand;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class DeleteGenreCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public DeleteGenreCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task DeleteGenreCommand()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var genre = GenreGenerator.GetGenre(new Guid());

            _context.Genres.Add(genre);
            _context.SaveChanges();

            var command = new DeleteGenreCommand()
            {
                GenreId = genre.GenreId,
            };

            await _mediatr.Send(command);

            var deleted = _context.Genres.FirstOrDefault(g => g.GenreId == command.GenreId);

            deleted.Should().BeNull();
        }
    }
}
