using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.UpdateBusinessCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class UpdateBusinessCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateBusinessCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateBusinessCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                })
            };

            _contextAccessor.HttpContext = httpContext;

            var BusinessGenerated = BusinessGenerator.GetGenericBusiness("AU", userID);

            _context.Business.Add(BusinessGenerated);
            _context.SaveChanges();

            var newBusinessDetails = BusinessGenerator.GetGenericBusiness("US", userID);

            var command = new UpdateBusinessCommand()
            {
                City = newBusinessDetails.City,
                CountryID = newBusinessDetails.CountryId,
                Name = newBusinessDetails.Name,
                Postcode = newBusinessDetails.Postcode,
                BusinessId = BusinessGenerated.BusinessId,
                BusinessTypeId = newBusinessDetails.BusinessTypeId,
                State = newBusinessDetails.State,
                StreetAddress = newBusinessDetails.StreetAddress,
                Website = newBusinessDetails.Website,
            };

            await _mediatr.Send(command);

            var Business = _context.Business.FirstOrDefault(p => p.BusinessId == BusinessGenerated.BusinessId);

            Business.Should().BeEquivalentTo(new Domain.Business()
            {
                City = newBusinessDetails.City,
                CountryId = newBusinessDetails.CountryId,
                CreatedBy = userID,
                CreatedDate = BusinessGenerated.CreatedDate,
                BusinessTypeId = newBusinessDetails.BusinessTypeId,
                IsDeleted = false,
                Name = newBusinessDetails.Name,
                Postcode = newBusinessDetails.Postcode,
                BusinessId = BusinessGenerated.BusinessId,
                State = newBusinessDetails.State,
                StreetAddress = newBusinessDetails.StreetAddress,
                Website = newBusinessDetails.Website,
            }, opt => opt.Excluding(f => f.Country).Excluding(f => f.Type));
        }
    }
}
