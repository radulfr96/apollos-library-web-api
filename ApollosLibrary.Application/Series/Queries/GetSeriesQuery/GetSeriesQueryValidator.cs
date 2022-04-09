using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Queries.GetSeriesQuery
{
    public class GetSeriesQueryValidator : AbstractValidator<GetSeriesQuery>
    {
        public GetSeriesQueryValidator()
        {
            RuleFor(c => c.SeriesId).GreaterThan(0).WithErrorCode(ErrorCodeEnum.SeriesIdInvalidValue.ToString());
        }
    }
}
