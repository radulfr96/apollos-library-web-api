using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Genre.Commands.AddGenreCommand;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddGenreCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddGenreCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task AddGenreCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;

            var genreGenerated = GenreGenerator.GetGenre(new Guid());

            var command = new AddGenreCommand()
            {
                Name = genreGenerated.Name,
            };

            var result = await _mediatr.Send(command);

            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == result.GenreID);

            genre.Should().NotBeNull();

            genre.Should().BeEquivalentTo(new Domain.Genre()
            {
                GenreId = genre.GenreId,
                CreatedDate = _dateTime.Now,
                CreatedBy = userID,
                Name = command.Name,
            });
        }
    }
}
