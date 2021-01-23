using FluentValidation;
using MyLibrary.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands.AddAuthorCommand
{
    public class AddAuthorCommandValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorCommandValidator()
        {
            RuleFor(a => a.Firstname).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());

            RuleFor(a => a.CountryID).NotEmpty().WithErrorCode(ErrorCodeEnum.CountryNotProvided.ToString());
        }
    }
}
