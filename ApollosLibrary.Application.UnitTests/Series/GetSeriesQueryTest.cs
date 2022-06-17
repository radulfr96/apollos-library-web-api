
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Series.Queries.GetSeriesQuery;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.UnitTests.Series
{
    [Collection("UnitTestCollection")]
    public class GetSeriesQueryTest : TestBase
    {
        private readonly GetSeriesQueryValidator _validator;

        public GetSeriesQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetSeriesQueryValidator();
        }

        [Fact]
        public void SeriesIdInvalidValue()
        {
            var command = new GetSeriesQuery();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.SeriesId);
        }

        [Fact]
        public async Task SeriesNotFound()
        {
            var query = new GetSeriesQuery()
            {
                SeriesId = 1,
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

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(s => s.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<SeriesNotFoundException>(() => mediator.Send(query));
        }
    }
}
