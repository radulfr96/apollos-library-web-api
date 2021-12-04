using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Genre.Commands.AddGenreCommand;
using MyLibrary.Application.IntegrationTests.Generators;
using MyLibrary.Application.Interfaces;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddGenreCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public AddGenreCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task AddGenreCommand()
        {
            var userID = new Guid();

            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, userID.ToString()),
            });

            var genreGenerated = GenreGenerator.GetGenre(new Guid());

            var command = new AddGenreCommand()
            {
                Name = genreGenerated.Name,
            };

            var result = await _mediatr.Send(command);

            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == result.GenreID);

            genre.Should().NotBeNull();

            genre.Should().BeEquivalentTo(new Persistence.Model.Genre()
            {
                GenreId = genre.GenreId,
                CreatedDate = _dateTime.Now,
                CreatedBy = userID,
                Name = command.Name,
            });
        }
    }
}
