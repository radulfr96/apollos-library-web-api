using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Author.Commands.AddAuthorCommand;
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
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddAuthorCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task AddAuthorCommandSuccess()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;
            
            var authorGenerated = AuthorGenerator.GetGenericAuthor(userID, "AU");

            var command = new AddAuthorCommand()
            {
                Firstname = authorGenerated.FirstName,
                Lastname = authorGenerated.LastName,
                Middlename = authorGenerated.MiddleName,
                CountryID = authorGenerated.CountryId,
                Description = authorGenerated.Description,
            };

            var result = await _mediatr.Send(command);

            var author = _context.Authors.Include(f => f.AuthorRecords).FirstOrDefault(a => a.AuthorId == result.AuthorId);

            author.Should().BeEquivalentTo(new Domain.Author()
            {
                AuthorId = author.AuthorId,
                CountryId = command.CountryID,
                CreatedBy = userID,
                CreatedDate = _dateTime.Now,
                Description = command.Description,
                FirstName = command.Firstname,
                LastName = command.Lastname,
                MiddleName = command.Middlename,
                VersionId = author.AuthorRecords.Last().AuthorRecordId,
            }, opt => opt.Excluding(a => a.Country).Excluding(a => a.Books).Excluding(f => f.AuthorRecords));

            author.AuthorRecords.First().Should().BeEquivalentTo(new AuthorRecord()
            {
                AuthorId = author.AuthorId,
                AuthorRecordId = author.AuthorRecords.First().AuthorRecordId,
                CountryId = authorGenerated.CountryId,
                CreatedBy = userID,
                CreatedDate = _dateTime.Now,
                Description = authorGenerated.Description,
                FirstName = authorGenerated.FirstName,
                IsDeleted = false,
                LastName = authorGenerated.LastName,
                MiddleName = authorGenerated.MiddleName,
                ReportedVersion = false,
            }, opt => opt.Excluding(f => f.Author).Excluding(f => f.Country));
        }
    }
}
