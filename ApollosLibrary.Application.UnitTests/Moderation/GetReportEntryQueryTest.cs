using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using Bogus;
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

namespace ApollosLibrary.Application.UnitTests.Moderation
{
    [Collection("UnitTestCollection")]
    public class GetReportEntryQueryTest : TestBase
    {
        private readonly GetEntryReportQueryValidator _validator;

        public GetReportEntryQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetEntryReportQueryValidator();
        }

        [Fact]
        public void EntryIdInvalidValue()
        {
            var command = new GetEntryReportQuery();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.EntryReportId);
        }

        [Fact]
        public async Task EntryReportNotFound()
        {
            var command = new GetEntryReportQuery()
            {
                EntryReportId = new Faker().Random.Int(1),
            };

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var moderationUnitOfWork = new Mock<IModerationUnitOfWork>();
            var moderationDataLayer = new Mock<IModerationDataLayer>();
            moderationDataLayer.Setup(s => s.GetEntryReport(It.IsAny<int>())).Returns(Task.FromResult((EntryReport)null));
            moderationUnitOfWork.Setup(s => s.ModerationDataLayer).Returns(moderationDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return moderationUnitOfWork.Object;
            });

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<EntryReportNotFoundException>();
        }
    }
}
