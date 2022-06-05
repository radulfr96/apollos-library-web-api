using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Commands.UpdateBusinessCommand
{
    public class UpdateBusinessCommandValidator : AbstractValidator<UpdateBusinessCommand>
    {
        public UpdateBusinessCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Name).Length(1, 200);

            When(p => !string.IsNullOrEmpty(p.Website), () =>
            {
                RuleFor(p => p.Website).Length(1, 200);
            });

            When(p => string.IsNullOrEmpty(p.Website), () =>
            {
                RuleFor(p => p).Must(BeValidAddress);
            });

            RuleFor(p => p.CountryID).NotEmpty();
        }

        private bool BeValidAddress(UpdateBusinessCommand command)
        {
            return !((string.IsNullOrEmpty(command.StreetAddress)
                    || string.IsNullOrEmpty(command.City)
                    || string.IsNullOrEmpty(command.Postcode)
                    || string.IsNullOrEmpty(command.State)
                    )
                    &&
                    (
                    string.IsNullOrEmpty(command.StreetAddress)
                    || string.IsNullOrEmpty(command.City)
                    || string.IsNullOrEmpty(command.Postcode)));
        }
    }
}
