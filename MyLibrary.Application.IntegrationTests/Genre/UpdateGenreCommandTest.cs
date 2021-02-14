using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Genre.Commands.UpdateGenreCommand;
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
    public class UpdateGenreCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public UpdateGenreCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task UpdateGenreCommand()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var genre = GenreGenerator.GetGenre(1);

            _context.Genres.Add(genre);
            _context.SaveChanges();

            var newGenreDetails = GenreGenerator.GetGenre(1);

            var command = new UpdateGenreCommand()
            {
                GenreId = genre.GenreId,
                Name = newGenreDetails.Name,
            };

            await _mediatr.Send(command);

            var genreResult = _context.Genres.FirstOrDefault(g => g.GenreId == command.GenreId);

            genreResult.Should().NotBeNull();

            genreResult.Should().BeEquivalentTo(new Persistence.Model.Genre()
            {
                CreatedBy = 1,
                CreatedDate = genre.CreatedDate,
                GenreId = genre.GenreId,
                ModifiedBy = 1,
                ModifiedDate = _dateTime.Now,
                Name = newGenreDetails.Name,
            });
        }
    }
}
