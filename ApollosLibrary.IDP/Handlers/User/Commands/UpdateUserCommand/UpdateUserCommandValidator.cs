using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.User.Commands.UpdateUserCommand
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Roles).NotEmpty();
            RuleFor(u => u).Must(u => BeValidPassword(u.Password, u.ConfirmationPassword));
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
