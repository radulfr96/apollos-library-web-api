using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ApollosLibrary.Application.Author.Commands.DeleteAuthorCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using FluentAssertions;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class DeleteAuthorCommandTest : TestBase
    {
        private readonly Faker _faker;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public DeleteAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            _faker = new Faker();
            var services = fixture.ServiceCollection;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task DeleteAuthorSuccess()
        {

            var author = new Domain.Author()
            {
                CountryId = "AU",
                CreatedBy = new Guid(),
                CreatedDate = new DateTime(2021, 02, 07),
                Description = _faker.Lorem.Sentence(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            var record = _context.Authors.FirstOrDefault(a => a.AuthorId == author.AuthorId);

            record.Should().NotBeNull();

            var command = new DeleteAuthorCommand()
            {
                AuthorId = author.AuthorId,
            };

            await _mediatr.Send(command);

            var authorAfter = _context.Authors.FirstOrDefault(a => a.AuthorId == command.AuthorId);

            authorAfter.Should().NotBeNull();
            authorAfter.IsDeleted.Should().BeTrue();
        }
    }
}
