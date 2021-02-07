using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Application.Author.Commands.DeleteAuthorCommand;
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
    public class DeleteAuthorCommandTest : TestBase
    {
        private readonly Faker _faker;

        public DeleteAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            _faker = new Faker();
        }

        [Fact]
        public async Task DeleteAuthorSuccess()
        {

            var author = new Persistence.Model.Author()
            {
                CountryId = "AU",
                CreatedBy = 1,
                CreatedDate = new DateTime(2021, 02, 07),
                Description = _faker.Lorem.Sentence(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                MiddleName = _faker.Name.FirstName(),
            };

            _fixture.context.Authors.Add(author);
            _fixture.context.SaveChanges();

            var command = new DeleteAuthorCommand()
            {
                AuthorId = author.AuthorId,
            };

            var services = _fixture.ServiceCollection;

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await mediator.Send(command);

            var authorAfter = _fixture.context.Authors.FirstOrDefault(a => a.AuthorId == command.AuthorId);

            Assert.Null(authorAfter);
        }
    }
}
