using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Interfaces;
using MyLibrary.Application.User.Commands.UpdatePasswordCommand;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class UpdatePasswordCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public UpdatePasswordCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task WhenUpdatePassword_PasswordUpdated()
        {
            var userID = new Guid();

            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, userID.ToString()),
            });

            var password = new Faker().Random.AlphaNumeric(50);

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
            user.Password = hasher.HashPassword(user, password);

            _context.Users.Add(user);
            _context.SaveChanges();

            var newPassword = new Faker().Random.AlphaNumeric(50);
            var hashedNewPassword = hasher.HashPassword(user, newPassword);

            var command = new UpdatePasswordCommand()
            {
                CurrentPassword = password,
                NewPassword = newPassword,
            };

            await _mediatr.Send(command);

            var updatedUser = _context.Users.FirstOrDefault(p => p.UserId == userID);

            updatedUser.Password.Should().BeEquivalentTo(hashedNewPassword);
        }
    }
}
