using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinessQuery
{
    public class GetBusinessQueryValidator : AbstractValidator<GetBusinessQuery>
    {
        public GetBusinessQueryValidator()
        {
            RuleFor(q => q.BusinessId).GreaterThan(0).WithErrorCode(ErrorCodeEnum.BusinessIdInvalidValue.ToString());
        }
    }
}
