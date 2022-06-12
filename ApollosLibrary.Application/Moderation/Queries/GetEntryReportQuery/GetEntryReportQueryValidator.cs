using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery
{
    public class GetEntryReportQueryValidator : AbstractValidator<GetEntryReportQuery>
    {
        public GetEntryReportQueryValidator()
        {
            RuleFor(q => q.ReportEntryId).GreaterThan(0);
        }
    }
}
