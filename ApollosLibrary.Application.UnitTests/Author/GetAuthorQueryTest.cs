using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Author.Queries.GetAuthorQuery;
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

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class GetAuthorQueryTest : TestBase
    {
        public GetAuthorQueryTest(TestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task GetAuthorAuthorNotFound()
        {
            var command = new GetAuthorQuery()
            {
                AuthorId = 1
            };

            var mockReferenceDataLayer = new Mock<IReferenceDataLayer>();
            var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

            var mockReferenceUOW = new Mock<IReferenceUnitOfWork>();
            mockReferenceUOW.Setup(u => u.ReferenceDataLayer).Returns(mockReferenceDataLayer.Object);

            var mockAuthorUow = new Mock<IAuthorUnitOfWork>();
            mockAuthorUow.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

            var mockUserService = new Mock<IUserService>();

            var mockDateTimeService = new Mock<IDateTimeService>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockReferenceUOW.Object;
            });

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

            await Assert.ThrowsAsync<AuthorNotFoundException>(() => mediator.Send(command));
        }
    }
}
