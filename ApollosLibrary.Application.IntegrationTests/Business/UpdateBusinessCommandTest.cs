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
using NodaTime;

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
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
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

            var businessGenerated = BusinessGenerator.GetGenericBusiness("AU", userID);
            _context.Business.Add(businessGenerated);
            _context.SaveChanges();

            var record = new BusinessRecord()
            {
                BusinessId = businessGenerated.BusinessId,
                BusinessTypeId = businessGenerated.BusinessTypeId,
                City = businessGenerated.City,
                CountryId = businessGenerated.CountryId,
                CreatedBy = businessGenerated.CreatedBy,
                CreatedDate = businessGenerated.CreatedDate,
                IsDeleted = businessGenerated.IsDeleted,
                Name = businessGenerated.Name,
                Postcode = businessGenerated.Postcode,
                ReportedVersion = false,
                State = businessGenerated.State,
                StreetAddress = businessGenerated.StreetAddress,
                Website = businessGenerated.Website,
            };
            _context.BusinessRecords.Add(record);
            _context.SaveChanges();

            businessGenerated.VersionId = record.BusinessRecordId;
            _context.SaveChanges();

            var newBusinessDetails = BusinessGenerator.GetGenericBusiness("US", userID);

            var command = new UpdateBusinessCommand()
            {
                City = newBusinessDetails.City,
                CountryID = newBusinessDetails.CountryId,
                Name = newBusinessDetails.Name,
                Postcode = newBusinessDetails.Postcode,
                BusinessId = businessGenerated.BusinessId,
                BusinessTypeId = newBusinessDetails.BusinessTypeId,
                State = newBusinessDetails.State,
                StreetAddress = newBusinessDetails.StreetAddress,
                Website = newBusinessDetails.Website,
            };

            await _mediatr.Send(command);

            var business = _context.Business.FirstOrDefault(p => p.BusinessId == businessGenerated.BusinessId);

            business.Should().BeEquivalentTo(new Domain.Business()
            {
                City = newBusinessDetails.City,
                CountryId = newBusinessDetails.CountryId,
                CreatedBy = userID,
                CreatedDate = businessGenerated.CreatedDate,
                BusinessTypeId = newBusinessDetails.BusinessTypeId,
                IsDeleted = false,
                Name = newBusinessDetails.Name,
                Postcode = newBusinessDetails.Postcode,
                BusinessId = businessGenerated.BusinessId,
                State = newBusinessDetails.State,
                StreetAddress = newBusinessDetails.StreetAddress,
                Website = newBusinessDetails.Website,
                VersionId = business.BusinessRecords.Last(r => r.BusinessId == business.BusinessId).BusinessRecordId,
            }, opt => opt.Excluding(f => f.Country).Excluding(f => f.Type).Excluding(f => f.BusinessRecords));

            business.BusinessRecords.Last(r => r.BusinessId == business.BusinessId).Should().BeEquivalentTo(new BusinessRecord()
            {
                BusinessRecordId = business.BusinessRecords.Last(r => r.BusinessId == business.BusinessId).BusinessRecordId,
                BusinessId = business.BusinessId,
                BusinessTypeId = newBusinessDetails.BusinessTypeId,
                City = newBusinessDetails.City,
                CountryId = newBusinessDetails.CountryId,
                CreatedBy = newBusinessDetails.CreatedBy,
                CreatedDate = business.CreatedDate,
                IsDeleted = false,
                Name = newBusinessDetails.Name,
                Postcode = newBusinessDetails.Postcode,
                ReportedVersion = false,
                State = newBusinessDetails.State,
                StreetAddress = newBusinessDetails.StreetAddress,
                Website = newBusinessDetails.Website,
            }, opt => opt.Excluding(f => f.Business));
        }
    }
}
