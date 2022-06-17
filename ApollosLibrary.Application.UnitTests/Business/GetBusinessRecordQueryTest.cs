using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Queries.GetBusinessQuery;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentValidation.TestHelper;

using FluentAssertions;
using ApollosLibrary.Application.Business.Queries.GetBusinessRecordQuery;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class GetBusinessRecordQueryTest : TestBase
    {
        private readonly GetBusinessRecordQueryValidator _validator;

        public GetBusinessRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetBusinessRecordQueryValidator();
        }

        [Fact]
        public void BusinessIdInvalidValue()
        {
            var query = new GetBusinessRecordQuery()
            {
                BusinessRecordId = 0,
            };

            var result = _validator.TestValidate(query);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BusinessRecordId);
        }

        [Fact]
        public async Task BusinessNotFound()
        {
            var query = new GetBusinessRecordQuery()
            {
                BusinessRecordId = 1,
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

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();
            BusinessUnitOfWork.Setup(s => s.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<BusinessRecordNotFoundException>(() => mediator.Send(query));
        }
    }
}
