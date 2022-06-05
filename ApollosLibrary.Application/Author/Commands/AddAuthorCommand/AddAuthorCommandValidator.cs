using FluentValidation;
using ApollosLibrary.Application.Common.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Commands.AddAuthorCommand
{
    public class AddAuthorCommandValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorCommandValidator()
        {
            RuleFor(a => a.Firstname).NotEmpty();
            RuleFor(a => a.Firstname).Length(1, 50);
            RuleFor(a => a.Firstname).Must(StringValidator.BeValidName);

            When(a => !string.IsNullOrEmpty(a.Middlename), () =>
            {
                RuleFor(a => a.Middlename).Length(1, 50);
                RuleFor(a => a.Middlename).Must(StringValidator.BeValidName);
            });

            RuleFor(a => a.Lastname).NotEmpty();
            RuleFor(a => a.Lastname).Length(1, 50);
            RuleFor(a => a.Lastname).Must(StringValidator.BeValidName);

            RuleFor(a => a.CountryID).NotEmpty();

            When(a => !string.IsNullOrEmpty(a.Description), () =>
            {
                RuleFor(a => a.Description).Length(1, 2000);
            });
        }
    }
}
