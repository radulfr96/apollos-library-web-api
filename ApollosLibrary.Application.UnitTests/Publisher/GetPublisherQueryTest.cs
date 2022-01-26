using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Publisher.Queries.GetPublisherQuery;
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
    public class GetPublisherQueryTest : TestBase
    {
        public GetPublisherQueryTest(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task PublisherNotFound()
        {
            var query = new GetPublisherQuery()
            {
                PublisherId = 1,
            };

            var mockUserService = new Mock<IUserService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var mockDateTimeService = new Mock<IDateTimeService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var publisherUnitOfWork = new Mock<IPublisherUnitOfWork>();

            var publisherDataLayer = new Mock<IPublisherDataLayer>();
            publisherUnitOfWork.Setup(s => s.PublisherDataLayer).Returns(publisherDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return publisherUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<PublisherNotFoundException>(() => mediator.Send(query));
        }
    }
}
