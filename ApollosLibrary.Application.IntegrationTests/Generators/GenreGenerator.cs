using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.IntegrationTests.Generators
{
    public static class GenreGenerator
    {
        public static Persistence.Model.Genre GetGenre(Guid createBy)
        {
            return new Faker<Persistence.Model.Genre>()
                .RuleFor(g => g.CreatedBy, createBy)
                .RuleFor(g => g.CreatedDate, f => f.Date.Recent())
                .RuleFor(g => g.Name, f => f.Name.Random.AlphaNumeric(8))
                .Generate();
        }
    }
}
