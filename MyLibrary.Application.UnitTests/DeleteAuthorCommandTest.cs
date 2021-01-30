using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Author.Commands.DeleteAuthorCommand;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Interfaces;
using MyLibrary.Application.XUnitTestProject;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class DeleteAuthorCommandTest : TestBase
    {
        private readonly Faker _faker;
        private readonly TestFixture _fixture1;

        public DeleteAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            _faker = new Faker();
            _fixture = fixture;
        }

        [Fact]
        public async Task AuthorNotFound()
        {
            var mockUserService = new Mock<IUserService>();

            var mockDateTimeService = new Mock<IDateTimeService>();

            var mockAuthorDatalayer = new Mock<IAuthorDataLayer>();
            mockAuthorDatalayer.Setup(a => a.GetAuthor(It.IsAny<int>())).Returns(Task.FromResult((Persistence.Model.Author)null));

            var mockAuthorUow = new Mock<IAuthorUnitOfWork>();
            mockAuthorUow.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDatalayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockAuthorUow.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var command = new DeleteAuthorCommand()
            {
                AuthorId = 1,
            };

            await Assert.ThrowsAsync<AuthorNotFoundException>(() => mediator.Send(command));
        }
    }
}
