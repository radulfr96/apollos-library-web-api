using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetLibraryEntryQuery
{
    public class GetLibraryEntryQueryValidator : AbstractValidator<GetLibraryEntryQuery>
    {
        public GetLibraryEntryQueryValidator()
        {
            RuleFor(q => q.EntryId).GreaterThan(0);
        }
    }
}
