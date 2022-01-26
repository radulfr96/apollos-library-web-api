using FluentValidation;
using ApollosLibrary.Application.Common.Enums;
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
            RuleFor(a => a.Firstname).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            RuleFor(a => a.Firstname).Length(1, 50).WithErrorCode(ErrorCodeEnum.FirstnameInvalidLength.ToString());
            RuleFor(a => a.Firstname).Must(StringValidator.BeValidName).WithErrorCode(ErrorCodeEnum.FirstnameInvalidFormat.ToString());

            When(a => !string.IsNullOrEmpty(a.Middlename), () =>
            {
                RuleFor(a => a.Middlename).Length(1, 50).WithErrorCode(ErrorCodeEnum.MiddlenameInvalidLength.ToString());
                RuleFor(a => a.Middlename).Must(StringValidator.BeValidName).WithErrorCode(ErrorCodeEnum.MiddlenameInvalidFormat.ToString());
            });

            RuleFor(a => a.Lastname).NotEmpty().WithErrorCode(ErrorCodeEnum.LastnameNotProvided.ToString());
            RuleFor(a => a.Lastname).Length(1, 50).WithErrorCode(ErrorCodeEnum.LastnameInvalidLength.ToString());
            RuleFor(a => a.Lastname).Must(StringValidator.BeValidName).WithErrorCode(ErrorCodeEnum.LastnameInvalidFormat.ToString());

            RuleFor(a => a.CountryID).NotEmpty().WithErrorCode(ErrorCodeEnum.CountryNotProvided.ToString());

            When(a => !string.IsNullOrEmpty(a.Description), () =>
            {
                RuleFor(a => a.Description).Length(1, 2000).WithErrorCode(ErrorCodeEnum.DecriptionInvalidLength.ToString());
            });
        }
    }
}
