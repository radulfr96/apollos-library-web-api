using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Author.Commands.DeleteAuthorCommand;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class DeleteAuthorCommandTest : TestBase
    {
        public DeleteAuthorCommandTest(TestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task AuthorNotFound()
        {
            var mockUserService = new Mock<IUserService>();

            var mockDateTimeService = new Mock<IDateTimeService>();

            var mockAuthorDatalayer = new Mock<IAuthorDataLayer>();
            mockAuthorDatalayer.Setup(a => a.GetAuthor(It.IsAny<int>())).Returns(Task.FromResult((Domain.Author)null));

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

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<AuthorNotFoundException>();
        }
    }
}
