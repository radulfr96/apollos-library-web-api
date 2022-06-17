using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Queries.GetAuthorRecordQuery
{
    public class GetAuthorRecordQueryValidator : AbstractValidator<GetAuthorRecordQuery>
    {
        public GetAuthorRecordQueryValidator()
        {
            RuleFor(q => q.AuthorRecordId).GreaterThan(0);
        }
    }
}
