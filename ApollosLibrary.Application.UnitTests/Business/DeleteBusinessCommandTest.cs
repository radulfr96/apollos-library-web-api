using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.DeleteBusinessCommand;
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

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class DeleteBusinessCommandTest : TestBase
    {
        private readonly DeleteBusinessCommandValidator _validator;

        public DeleteBusinessCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new DeleteBusinessCommandValidator();
        }

        [Fact]
        public void BusinessIdInvalidValue()
        {
            var command = new DeleteBusinessCommand()
            {
                BusinessId = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.BusinessId);
        }

        [Fact]
        public async Task BusinessNotFound()
        {
            var command = new DeleteBusinessCommand()
            {
                BusinessId = 1,
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

            await Assert.ThrowsAsync<BusinessNotFoundException>(() => mediator.Send(command));
        }
    }
}
