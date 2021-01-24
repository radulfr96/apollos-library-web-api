using FluentValidation;
using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Commands.AddPublisherCommand
{
    public class AddPublisherCommandValidator : AbstractValidator<AddPublisherCommand>
    {
        public AddPublisherCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            RuleFor(p => p.CountryID).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            When(p => string.IsNullOrEmpty(p.Website), () =>
            {
                RuleFor(p => p).Must(BeValidAddress).WithErrorCode(ErrorCodeEnum.CountryNotProvided.ToString());
            });
        }

        private bool BeValidAddress(AddPublisherCommand command)
        {
            return !(!string.IsNullOrEmpty(command.StreetAddress)
                    || !string.IsNullOrEmpty(command.City)
                    || !string.IsNullOrEmpty(command.Postcode)
                    || !string.IsNullOrEmpty(command.State)
                    )
                    &&
                    (
                    string.IsNullOrEmpty(command.StreetAddress)
                    || string.IsNullOrEmpty(command.City)
                    || string.IsNullOrEmpty(command.Postcode));
        }
    }
}
