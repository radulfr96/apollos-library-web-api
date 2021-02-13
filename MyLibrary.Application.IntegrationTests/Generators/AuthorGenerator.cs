using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.IntegrationTests.Generators
{
    public static class AuthorGenerator
    {
        public static Persistence.Model.Author GetGenericAuthor(int userId, string countryId)
        {
            return new Faker<Persistence.Model.Author>()
                .RuleFor(a => a.CountryId, countryId)
                .RuleFor(a => a.CreatedBy, userId)
                .RuleFor(a => a.CreatedDate, f => f.Date.Recent())
                .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                .RuleFor(a => a.FirstName, f => f.Name.FirstName())
                .RuleFor(a => a.LastName, f => f.Name.LastName())
                .RuleFor(a => a.MiddleName, f => f.Name.LastName())
                .Generate();
        }
    }
}
