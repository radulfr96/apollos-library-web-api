using Bogus;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Interfaces;
using MyLibrary.Application.User.Commands.UpdatePasswordCommand;
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
    public class UpdatePasswordCommandTest : TestBase
    {
        public UpdatePasswordCommandTest(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task WhenUserIsNotFound_ReturnNotFoundException()
        {
            var command = new UpdatePasswordCommand()
            {
                CurrentPassword = new Faker().Random.AlphaNumeric(50),
                NewPassword = new Faker().Random.AlphaNumeric(50),
            };

            var mockDateTimeService = new Mock<IDateTimeService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var mockUserService = new Mock<IUserService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var userDataLayer = new Mock<IUserDataLayer>();

            var mockUserUOW = new Mock<IUserUnitOfWork>();
            mockUserUOW.Setup(p => p.UserDataLayer).Returns(userDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserUOW.Object;
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

            await Assert.ThrowsAsync<UserNotFoundException>(() => mediator.Send(command));
        }

        [Fact]
        public async Task WhenCurrentPasswordNotCorrect_ReturnPasswordInvalidException()
        {
            var command = new UpdatePasswordCommand()
            {
                CurrentPassword = new Faker().Random.AlphaNumeric(50),
                NewPassword = new Faker().Random.AlphaNumeric(50),
            };

            var mockDateTimeService = new Mock<IDateTimeService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var mockUserService = new Mock<IUserService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var hasher = new PasswordHasher<Persistence.Model.User>();
            var user = new Persistence.Model.User()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.Parse("2021-01-02"),
                IsActive = true,
                Subject = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Username = new Faker().Internet.UserName(),
            };

            user.Password = hasher.HashPassword(user, new Faker().Random.AlphaNumeric(50));

            var userDataLayer = new Mock<IUserDataLayer>();
            userDataLayer.Setup(u => u.GetUser(It.IsAny<Guid>())).Returns(Task.FromResult(user));

            var mockUserUOW = new Mock<IUserUnitOfWork>();
            mockUserUOW.Setup(p => p.UserDataLayer).Returns(userDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserUOW.Object;
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

            await Assert.ThrowsAsync<PasswordInvalidException>(() => mediator.Send(command));
        }
    }
}
