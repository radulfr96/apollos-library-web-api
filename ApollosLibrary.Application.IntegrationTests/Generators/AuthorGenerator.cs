using Bogus;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.IntegrationTests.Generators
{
    public static class AuthorGenerator
    {
        public static Domain.Author GetGenericAuthor(Guid userId, string countryId)
        {
            return new Faker<Domain.Author>()
                .RuleFor(a => a.CountryId, countryId)
                .RuleFor(a => a.CreatedBy, userId)
                .RuleFor(a => a.CreatedDate, f =>  LocalDateTime.FromDateTime(f.Date.Recent()))
                .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                .RuleFor(a => a.FirstName, f => f.Name.FirstName())
                .RuleFor(a => a.LastName, f => f.Name.LastName())
                .RuleFor(a => a.MiddleName, f => f.Name.LastName())
                .Generate();
        }
    }
}
