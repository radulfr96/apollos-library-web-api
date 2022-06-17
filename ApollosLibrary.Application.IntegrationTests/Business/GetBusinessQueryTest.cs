using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Queries.GetBusinessQuery;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Business.Queries.GetBusinessRecordQuery;
using System.Collections.Generic;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetBusinessRecordQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBusinessRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task GetBusinessRecordQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var businessGenerated = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            var businessRecord = new BusinessRecord()
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
                ReportedVersion = true,
                State = businessGenerated.State,
                StreetAddress = businessGenerated.StreetAddress,
                Website = businessGenerated.Website,
            };

            businessGenerated.BusinessRecords = new List<BusinessRecord>()
            {
                businessRecord
            };

            _context.Business.Add(businessGenerated);
            _context.SaveChanges();

            var query = new GetBusinessRecordQuery()
            {
                BusinessRecordId = businessGenerated.BusinessId,
            };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetBusinessRecordQueryDto()
            {
                City = businessGenerated.City,
                Name = businessGenerated.Name,
                Postcode = businessGenerated.Postcode,
                BusinessId = businessGenerated.BusinessId,
                State = businessGenerated.State,
                StreetAddress = businessGenerated.StreetAddress,
                Website = businessGenerated.Website,
            });
        }
    }
}
