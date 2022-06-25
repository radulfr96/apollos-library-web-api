using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Author.Commands.UpdateAuthorCommand;
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

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class UpdateAuthorCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly ServiceProvider _provider;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            fixture.ServiceCollection.AddTransient(p =>
            {
                return _dateTime;
            });

            _provider = fixture.ServiceCollection.BuildServiceProvider();
            _mediatr = _provider.GetRequiredService<IMediator>();
            _context = _provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = _provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateAuthorCommandSuccess()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;

            var author = AuthorGenerator.GetGenericAuthor(userID, "US");

            _context.Authors.Add(author);
            _context.SaveChanges();

            var newAuthorDetails = AuthorGenerator.GetGenericAuthor(userID, "AU");

            var command = new UpdateAuthorCommand()
            {
                Firstname = newAuthorDetails.FirstName,
                Lastname = newAuthorDetails.LastName,
                Middlename = newAuthorDetails.MiddleName,
                CountryID = newAuthorDetails.CountryId,
                Description = newAuthorDetails.Description,
                AuthorId = author.AuthorId,
            };

            var result = await _mediatr.Send(command);

            var authorRecord = _context.Authors.Include(a => a.AuthorRecords).FirstOrDefault(a => a.AuthorId == author.AuthorId);

            authorRecord.Should().BeEquivalentTo(new Domain.Author()
            {
                AuthorId = author.AuthorId,
                CountryId = newAuthorDetails.CountryId,
                CreatedBy = userID,
                CreatedDate = author.CreatedDate,
                Description = newAuthorDetails.Description,
                FirstName = newAuthorDetails.FirstName,
                LastName = newAuthorDetails.LastName,
                MiddleName = newAuthorDetails.MiddleName,
                VersionId = author.AuthorRecords.Last().AuthorRecordId,
            }, opt => opt.Excluding(a => a.Country).Excluding(a => a.Books).Excluding(f => f.AuthorRecords));

            author.AuthorRecords.Where(a => a.AuthorId == author.AuthorId).First().Should().BeEquivalentTo(new AuthorRecord()
            {
                AuthorId = author.AuthorId,
                AuthorRecordId = author.AuthorRecords.First().AuthorRecordId,
                CountryId = command.CountryID,
                CreatedBy = userID,
                CreatedDate = _dateTime.Now,
                Description = command.Description,
                FirstName = command.Firstname,
                LastName = command.Lastname,
                IsDeleted = false,
                MiddleName = command.Middlename,
                ReportedVersion = false,
            }, opt => opt.Excluding(f => f.Author).Excluding(f => f.Country));
        }
    }
}
