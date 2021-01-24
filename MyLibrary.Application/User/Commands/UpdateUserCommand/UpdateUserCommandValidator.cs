using FluentValidation;
using MyLibrary.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Commands.UpdateUserCommand
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.Username).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            RuleFor(u => u.Roles).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            RuleFor(u => u).Must(u => BeValidPassword(u.Password, u.ConfirmationPassword)).WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
        }

        private bool BeValidPassword(string password, string confirmationPassword)
        {
            if (!Regex.IsMatch(password, "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"))
            {
                return false;
            }

            if (password != confirmationPassword)
            {
                return false;
            }

            return true;
        }
    }
}
