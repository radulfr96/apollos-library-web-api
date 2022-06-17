using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBookRecordQuery
{
    public class GetBookRecordQueryValidator : AbstractValidator<GetBookRecordQuery>
    {
        public GetBookRecordQueryValidator()
        {
            RuleFor(q => q.BookRecordId).GreaterThan(0);
        }
    }
}
