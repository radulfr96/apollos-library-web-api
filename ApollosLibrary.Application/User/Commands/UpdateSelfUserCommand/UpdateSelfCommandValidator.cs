using FluentValidation;
using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.User.Commands.UpdateSelfUserCommand
{
    public class UpdateSelfCommandValidator : AbstractValidator<UpdateSelfUserCommand>
    {
        public UpdateSelfCommandValidator()
        {
            RuleFor(u => u.Username).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
        }
    }
}
