using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Author.Commands.AddAuthorCommand;
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
        private readonly Faker _faker;
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public AddAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            _faker = new Faker();
            var services = fixture.ServiceCollection;

            _faker = new Faker();
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

            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                CountryID = "AU",
                Description = _faker.Lorem.Sentence(),
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
