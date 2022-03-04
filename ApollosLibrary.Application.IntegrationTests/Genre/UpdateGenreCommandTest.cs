using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Genre.Commands.UpdateGenreCommand;
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
    public class UpdateGenreCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContextOld _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateGenreCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContextOld>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateGenreCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;

            var genre = GenreGenerator.GetGenre(userID);

            _context.Genres.Add(genre);
            _context.SaveChanges();

            var newGenreDetails = GenreGenerator.GetGenre(new Guid());

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
                CreatedBy = userID,
                CreatedDate = genre.CreatedDate,
                GenreId = genre.GenreId,
                ModifiedBy = userID,
                ModifiedDate = _dateTime.Now,
                Name = newGenreDetails.Name,
            });
        }
    }
}
