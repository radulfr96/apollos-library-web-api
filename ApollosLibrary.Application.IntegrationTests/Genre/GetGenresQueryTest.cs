using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Genre.Queries.GetGenresQuery;
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
    public class GetGenresQueryTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetGenresQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetGenresQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var genre1 = GenreGenerator.GetGenre(new Guid());

            _context.Genres.Add(genre1);

            var genre2 = GenreGenerator.GetGenre(new Guid());

            _context.Genres.Add(genre2);

            _context.SaveChanges();

            var query = new GetGenresQuery() { };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetGenresQueryDto()
            {
                Genres = new List<GenreDto>()
                {
                    new GenreDto()
                    {
                        GenreId = genre1.GenreId,
                        Name =  genre1.Name,
                    },
                    new GenreDto()
                    {
                        GenreId = genre2.GenreId,
                        Name = genre2.Name,
                    }
                }
            });
        }
    }
}
