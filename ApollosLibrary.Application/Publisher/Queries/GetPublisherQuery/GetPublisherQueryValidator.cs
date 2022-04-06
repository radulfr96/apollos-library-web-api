using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Publisher.Queries.GetPublisherQuery
{
    public class GetPublisherQueryValidator : AbstractValidator<GetPublisherQuery>
    {
        public GetPublisherQueryValidator()
        {
            RuleFor(q => q.PublisherId).GreaterThan(0).WithErrorCode(ErrorCodeEnum.PublisherIdInvalidValue.ToString());
        }
    }
}
