
using ApollosLibrary.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.IntegrationTests.Generators
{
    public static class BusinessGenerator
    {
        public static Domain.Business GetGenericBusiness(string countryId, Guid userId)
        {
            return new Faker<Domain.Business>()
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.CountryId, countryId)
                .RuleFor(p => p.BusinessTypeId, f => (int)f.Random.Enum<BusinessTypeEnum>())
                .RuleFor(p => p.CreatedBy, userId)
                .RuleFor(p => p.CreatedDate, f => f.Date.Recent())
                .RuleFor(p => p.IsDeleted, false)
                .RuleFor(p => p.Name, f => f.Company.CompanyName())
                .RuleFor(p => p.Postcode, f => f.Address.ZipCode())
                .RuleFor(p => p.State, f => f.Address.State())
                .RuleFor(p => p.StreetAddress, f => f.Address.StreetAddress())
                .RuleFor(p => p.Website, f => f.Internet.Url())
                .Generate();
        }
    }
}
