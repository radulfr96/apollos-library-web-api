using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.IntegrationTests.Generators
{
    public static class PublisherGenerator
    {
        public static Persistence.Model.Publisher GetGenericPublisher(string countryId)
        {
            return new Faker<Persistence.Model.Publisher>()
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.CountryId, countryId)
                .RuleFor(p => p.CreatedBy, 1)
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
