using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBookQuery
{
    public class GetBookQueryValidator : AbstractValidator<GetBookQuery>
    {
        public GetBookQueryValidator()
        {
            RuleFor(b => b.BookId).GreaterThan(0).WithErrorCode(ErrorCodeEnum.BookIdInvalidValue.ToString());
        }
    }
}
