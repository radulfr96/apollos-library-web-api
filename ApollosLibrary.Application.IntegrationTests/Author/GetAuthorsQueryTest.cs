using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ApollosLibrary.Application.Author.Queries.GetAuthorsQuery;
using ApollosLibrary.Application.IntegrationTests.Generators;
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
    public class GetAuthorsQueryTest : TestBase
    {
        private readonly ApollosLibraryContextOld _context;
        private readonly IMediator _mediatr;

        public GetAuthorsQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContextOld>();
        }

        [Fact]
        public async Task GetAuthor()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var authorGenerated1 = AuthorGenerator.GetGenericAuthor(new Guid(), "AU");
            var authorGenerated2 = AuthorGenerator.GetGenericAuthor(new Guid(), "US");
            var authorGenerated3 = AuthorGenerator.GetGenericAuthor(new Guid(), "UK");

            var country1 = _context.Countries.FirstOrDefault(c => c.CountryId == authorGenerated1.CountryId);
            var country2 = _context.Countries.FirstOrDefault(c => c.CountryId == authorGenerated2.CountryId);
            var country3 = _context.Countries.FirstOrDefault(c => c.CountryId == authorGenerated3.CountryId);

            _context.Authors.Add(authorGenerated1);
            _context.Authors.Add(authorGenerated2);
            _context.Authors.Add(authorGenerated3);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetAuthorsQuery() { });

            result.Should().BeEquivalentTo(new GetAuthorsQueryDto()
            {
                Authors = new List<AuthorListItemDTO>()
                {
                    new AuthorListItemDTO()
                    {
                        AuthorId = authorGenerated1.AuthorId,
                        Country = country1.Name,
                        Name = $"{authorGenerated1.FirstName} {authorGenerated1.MiddleName} {authorGenerated1.LastName}"
                    },
                    new AuthorListItemDTO()
                    {
                        AuthorId = authorGenerated2.AuthorId,
                        Country = country2.Name,
                        Name = $"{authorGenerated2.FirstName} {authorGenerated2.MiddleName} {authorGenerated2.LastName}"
                    },
                    new AuthorListItemDTO()
                    {
                        AuthorId = authorGenerated3.AuthorId,
                        Country = country3.Name,
                        Name = $"{authorGenerated3.FirstName} {authorGenerated3.MiddleName} {authorGenerated3.LastName}"
                    },
                }
            });
        }
    }
}
