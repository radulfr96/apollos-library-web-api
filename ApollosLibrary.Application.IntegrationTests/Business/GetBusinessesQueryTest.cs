using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Queries.GetBusinesssQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Common.Enums;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetBusinessesQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBusinessesQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetBusinessesQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var business1 = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(business1);

            var business2 = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(business2);

            var business3 = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(business3);

            _context.SaveChanges();

            var countries = _context.Countries.ToList();

            var query = new GetBusinesssQuery()
            {
            };

            var result = await _mediatr.Send(query);

            result.Businesses.Should().ContainEquivalentOf(new BusinessListItemDTO()
            {
                Country = countries.First(c => c.CountryId == business1.CountryId).Name,
                Name = business1.Name,
                BusinessId = business1.BusinessId,
                Type = GetBusinessTypeName((BusinessTypeEnum)business1.BusinessTypeId),
            });

            result.Businesses.Should().ContainEquivalentOf(new BusinessListItemDTO()
            {
                Country = countries.First(c => c.CountryId == business2.CountryId).Name,
                Name = business2.Name,
                BusinessId = business2.BusinessId,
                Type = GetBusinessTypeName((BusinessTypeEnum)business2.BusinessTypeId),
            });

            result.Businesses.Should().ContainEquivalentOf(new BusinessListItemDTO()
            {
                Country = countries.First(c => c.CountryId == business3.CountryId).Name,
                Name = business3.Name,
                BusinessId = business3.BusinessId,
                Type = GetBusinessTypeName((BusinessTypeEnum)business3.BusinessTypeId),
            });
        }

        public string GetBusinessTypeName(BusinessTypeEnum type)
        {
            switch(type)
            {
                case BusinessTypeEnum.Bookshop:
                    return "Bookshop";
                case BusinessTypeEnum.Publisher:
                    return "Publisher";
            }

            return "";
        }
    }
}
