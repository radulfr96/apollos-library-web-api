using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Author.Commands.UpdateAuthorCommand;
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

namespace MyLibrary.Application.IntegrationTests.Author
{
    [Collection("IntegrationTestCollection")]
    public class UpdateAuthorCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly ServiceProvider _provider;

        public UpdateAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            fixture.ServiceCollection.AddSingleton(mockDateTimeService.Object);

            
            _provider = fixture.ServiceCollection.BuildServiceProvider();
            _mediatr = _provider.GetRequiredService<IMediator>();
            _context = _provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task UpdateAuthorCommandSuccess()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var author = AuthorGenerator.GetGenericAuthor(1, "US");

            _context.Authors.Add(author);
            _context.SaveChanges();

            var newAuthorDetails = AuthorGenerator.GetGenericAuthor(1, "AU");

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

            var authorRecord = _context.Authors.FirstOrDefault(a => a.AuthorId == author.AuthorId);

            authorRecord.Should().BeEquivalentTo(new Persistence.Model.Author()
            {
                AuthorId = author.AuthorId,
                CountryId = newAuthorDetails.CountryId,
                CreatedBy = 1,
                CreatedDate = _dateTime.Now,
                Description = newAuthorDetails.Description,
                FirstName = newAuthorDetails.FirstName,
                LastName = newAuthorDetails.LastName,
                MiddleName = newAuthorDetails.MiddleName,
                ModifiedBy = 1,
                ModifiedDate = _dateTime.Now,
            }, opt => opt.Excluding(a => a.Country).Excluding(a => a.BookAuthors));
        }
    }
}
