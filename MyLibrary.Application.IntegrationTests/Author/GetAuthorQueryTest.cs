using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Application.Author.Queries.GetAuthorQuery;
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
    public class GetAuthorQueryTest : TestBase
    {
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetAuthorQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task GetAuthor()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var authorGenerated = AuthorGenerator.GetGenericAuthor(new Guid(), "AU");

            _context.Authors.Add(authorGenerated);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetAuthorQuery()
            {
                AuthorId = authorGenerated.AuthorId
            });

            result.Should().BeEquivalentTo(new GetAuthorQueryDto()
            {
                AuthorID = authorGenerated.AuthorId,
                CountryID = authorGenerated.CountryId,
                Description = authorGenerated.Description,
                Firstname = authorGenerated.FirstName,
                Lastname = authorGenerated.LastName,
                Middlename = authorGenerated.MiddleName,
            });
        }
    }
}
