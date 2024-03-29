﻿
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Queries.GetAuthorQuery
{
    public class GetAuthorQueryValidator : AbstractValidator<GetAuthorQuery>
    {
        public GetAuthorQueryValidator()
        {
            RuleFor(q => q.AuthorId).GreaterThan(0);
        }
    }
}
