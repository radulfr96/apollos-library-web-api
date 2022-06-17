using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ApollosLibrary.Application.Author.Queries.GetAuthorQuery;
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
using ApollosLibrary.Application.Author.Queries.GetAuthorRecordQuery;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetAuthorRecordQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetAuthorRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task GetAuthorRecord()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var authorGenerated = AuthorGenerator.GetGenericAuthor(new Guid(), "AU");
            var authorRecord = new AuthorRecord()
            {
                AuthorId = authorGenerated.AuthorId,
                CountryId = authorGenerated.CountryId,
                CreatedBy = authorGenerated.CreatedBy,
                CreatedDate = authorGenerated.CreatedDate,
                Description = authorGenerated.Description,
                FirstName = authorGenerated.FirstName,
                IsDeleted = authorGenerated.IsDeleted,
                LastName = authorGenerated.LastName,
                MiddleName = authorGenerated.MiddleName,
                ReportedVersion = true,
            };

            authorGenerated.AuthorRecords = new List<AuthorRecord>()
            {
                authorRecord
            };

            _context.Authors.Add(authorGenerated);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetAuthorRecordQuery()
            {
                AuthorRecordId = authorRecord.AuthorRecordId,
            });

            result.Should().BeEquivalentTo(new GetAuthorRecordQueryDto()
            {
                Description = authorGenerated.Description,
                FirstName = authorGenerated.FirstName,
                LastName = authorGenerated.LastName,
                MiddleName = authorGenerated.MiddleName,
                AuthorId = authorGenerated.AuthorId,
            });
        }
    }
}
