using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Commands.DeleteBusinessCommand
{
    public class DeleteBusinessCommandValidator : AbstractValidator<DeleteBusinessCommand>
    {
        public DeleteBusinessCommandValidator()
        {
            RuleFor(c => c.BusinessId).GreaterThan(0);
        }
    }
}
