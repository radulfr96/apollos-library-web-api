using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Queries.GetSeriesRecordQuery
{
    public class GetSeriesRecordQueryValidator : AbstractValidator<GetSeriesRecordQuery>
    {
        public GetSeriesRecordQueryValidator()
        {
            RuleFor(q => q.SeriesRecordId).GreaterThan(0);
        }
    }
}
