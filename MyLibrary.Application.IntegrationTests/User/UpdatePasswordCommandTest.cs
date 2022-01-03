using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _contextAccessor;

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
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task WhenUpdatePassword_PasswordUpdated()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;

            var password = new Faker().Random.AlphaNumeric(50);

            var hasher = new PasswordHasher<Persistence.Model.User>();
            var user = new Persistence.Model.User()
            {
                CreatedBy = userID,
                CreatedDate = DateTime.Parse("2021-01-02"),
                IsActive = true,
                Subject = Guid.NewGuid().ToString(),
                UserId = userID,
                Username = new Faker().Internet.UserName(),
            };
            var oldPasswordHash = hasher.HashPassword(user, password);
            user.Password = oldPasswordHash;

            _context.Users.Add(user);
            _context.SaveChanges();

            var savedUser = _context.Users.FirstOrDefault(p => p.UserId == userID);

            var newPassword = new Faker().Random.AlphaNumeric(50);

            var command = new UpdatePasswordCommand()
            {
                CurrentPassword = password,
                NewPassword = newPassword,
            };

            await _mediatr.Send(command);

            var updatedUser = _context.Users.FirstOrDefault(p => p.UserId == userID);
            var validateChangeResult = hasher.VerifyHashedPassword(updatedUser, updatedUser.Password, newPassword);

            validateChangeResult.Should().BeEquivalentTo(PasswordVerificationResult.Success);
            updatedUser.ModifiedBy.Should().Be(userID);
            updatedUser.ModifiedDate.Should().NotBeNull();
        }
    }
}
