using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.AddBusinessCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddBusinessCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddBusinessCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task AddBusinessCommand()
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

            var businessGenerated = BusinessGenerator.GetGenericBusiness("AU", userID);

            var command = new AddBusinessCommand()
            {
                City = businessGenerated.City,
                CountryID = businessGenerated.CountryId,
                Name = businessGenerated.Name,
                BusinessTypeId = businessGenerated.BusinessTypeId,
                Postcode = businessGenerated.Postcode,
                State = businessGenerated.State,
                StreetAddress = businessGenerated.StreetAddress,
                Website = businessGenerated.Website,
            };

            var result = await _mediatr.Send(command);

            var business = _context.Business.Include(b => b.BusinessRecords).FirstOrDefault(p => p.BusinessId == result.BusinessId);

            business.Should().BeEquivalentTo(new Domain.Business()
            {
                City = businessGenerated.City,
                CountryId = businessGenerated.CountryId,
                CreatedBy = businessGenerated.CreatedBy,
                BusinessTypeId = businessGenerated.BusinessTypeId,
                CreatedDate = _dateTime.Now,
                IsDeleted = false,
                Name = businessGenerated.Name,
                Postcode = businessGenerated.Postcode,
                BusinessId = result.BusinessId,
                State = businessGenerated.State,
                StreetAddress = businessGenerated.StreetAddress,
                Website = businessGenerated.Website,
            }, opt => opt.Excluding(f => f.Country).Excluding(f => f.Type).Excluding(f => f.BusinessRecords));

            business.BusinessRecords.First().Should().BeEquivalentTo(new BusinessRecord()
            {
                BusinessId = business.BusinessId,
                BusinessRecordId = 1,
                BusinessTypeId = businessGenerated.BusinessTypeId,
                City = businessGenerated.City,
                CountryId = businessGenerated.CountryId,
                CreatedBy = userID,
                CreatedDate = _dateTime.Now,
                IsDeleted = false,
                Name = businessGenerated.Name,
                Postcode = businessGenerated.Postcode,
                ReportedVersion = false,
                State = businessGenerated.State,
                StreetAddress = businessGenerated.StreetAddress,
                Website = businessGenerated.Website,
            }, opt => opt.Excluding(f => f.Business));
        }
    }
}
