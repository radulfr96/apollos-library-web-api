using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Author.Commands.AddAuthorCommand;
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
    public class AddAuthorCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public AddAuthorCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task AddAuthorCommandSuccess()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var authorGenerated = AuthorGenerator.GetGenericAuthor(1, "AU");

            var command = new AddAuthorCommand()
            {
                Firstname = authorGenerated.FirstName,
                Lastname = authorGenerated.LastName,
                Middlename = authorGenerated.MiddleName,
                CountryID = authorGenerated.CountryId,
                Description = authorGenerated.Description,
            };

            var result = await _mediatr.Send(command);

            var author = _context.Authors.FirstOrDefault(a => a.AuthorId == result.AuthorId);

            author.Should().BeEquivalentTo(new Persistence.Model.Author()
            {
                AuthorId = author.AuthorId,
                CountryId = command.CountryID,
                CreatedBy = 1,
                CreatedDate = _dateTime.Now,
                Description = command.Description,
                FirstName = command.Firstname,
                LastName = command.Lastname,
                MiddleName = command.Middlename,
            }, opt => opt.Excluding(a => a.Country).Excluding(a => a.BookAuthors));
        }
    }
}
