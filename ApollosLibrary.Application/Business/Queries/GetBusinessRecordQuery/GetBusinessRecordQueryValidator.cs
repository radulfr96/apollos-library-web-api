using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinessRecordQuery
{
    public class GetBusinessRecordQueryValidator : AbstractValidator<GetBusinessRecordQuery>
    {
        public GetBusinessRecordQueryValidator()
        {
            RuleFor(q => q.BusinessRecordId).GreaterThan(0);
        }
    }
}
