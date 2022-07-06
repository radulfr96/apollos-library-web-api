using Bogus;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.IntegrationTests.Generators
{
    internal class SeriesGenerator
    {
        public static Domain.Series GetSeries(Guid createBy)
        {
            var faker = new Faker();

            return new Faker<Domain.Series>()
                .RuleFor(b => b.CreatedBy, createBy)
                .RuleFor(a => a.CreatedDate, f => LocalDateTime.FromDateTime(f.Date.Recent()))
                .RuleFor(b => b.Name, f => f.Name.Random.AlphaNumeric(8))
                .Generate();
        }

        public static Domain.Series GetSeriesNoOrders(Guid createBy)
        {
            var faker = new Faker();

            return new Faker<Domain.Series>()
                .RuleFor(b => b.CreatedBy, createBy)
                .RuleFor(a => a.CreatedDate, f => LocalDateTime.FromDateTime(f.Date.Recent()))
                .RuleFor(b => b.Name, f => f.Name.Random.AlphaNumeric(8))
                .Generate();
        }
    }
}
