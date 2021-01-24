using FluentValidation;
using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(a => a.Firstname).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());

            RuleFor(a => a.CountryID).NotEmpty().WithErrorCode(ErrorCodeEnum.CountryNotProvided.ToString());
        }
    }
}
