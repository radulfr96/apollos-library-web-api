using ApollosLibrary.Domain.Enums;
using Bogus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.IntegrationTests.Generators
{
    public static class BookGenerator
    {
        public static Domain.Book GetGenericPhysicalBook(Guid createdBy)
        {
            return new Faker<Domain.Book>()
                .RuleFor(b => b.CreatedBy, createdBy)
                .RuleFor(b => b.CreatedDate, f => f.Date.Recent())
                .RuleFor(b => b.Edition, f => f.Random.Int(1, 100))
                .RuleFor(b => b.FictionTypeId, f => (int)f.Random.Enum<FictionTypeEnum>())
                .RuleFor(b => b.FormTypeId, f => (int)f.Random.Enum<FormTypeEnum>())
                .RuleFor(b => b.Isbn, f => f.Random.String2(12, "0123456789"))
                .RuleFor(b => b.PublicationFormatId, f => (int)PublicationFormatEnum.Printed)
                .RuleFor(b => b.Subtitle, f => f.Random.Words(1))
                .RuleFor(b => b.Title, f => f.Random.Words(1))
                .Generate();
        }

        public static Domain.Book GetGenericDigitalBook(Guid createdBy)
        {
            return new Faker<Domain.Book>()
                .RuleFor(b => b.CreatedBy, createdBy)
                .RuleFor(b => b.CreatedDate, f => f.Date.Recent())
                .RuleFor(b => b.Edition, f => f.Random.Int(1, 100))
                .RuleFor(b => b.FictionTypeId, f => (int)f.Random.Enum<FictionTypeEnum>())
                .RuleFor(b => b.FormTypeId, f => (int)f.Random.Enum<FormTypeEnum>())
                .RuleFor(b => b.EIsbn, f => f.Random.String2(12, "0123456789"))
                .RuleFor(b => b.PublicationFormatId, f => (int)PublicationFormatEnum.eBook)
                .RuleFor(b => b.Subtitle, f => f.Random.Words(1))
                .RuleFor(b => b.Title, f => f.Random.Words(1))
                .Generate();
        }
    }
}
