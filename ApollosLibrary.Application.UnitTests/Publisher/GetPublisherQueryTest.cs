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
using FluentValidation.TestHelper;
using ApollosLibrary.Application.Common.Enums;
using FluentAssertions;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class GetPublisherQueryTest : TestBase
    {
        private readonly GetPublisherQueryValidator _validator;

        public GetPublisherQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetPublisherQueryValidator();
        }

        [Fact]
        public void PublisherIdInvalidValue()
        {
            var command = new GetPublisherQuery()
            {
                PublisherId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.PublisherIdInvalidValue.ToString()).Any().Should().BeTrue();
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
