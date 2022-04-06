using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Publisher.Commands.DeletePublisherCommand
{
    public class DeletePublisherCommandValidator : AbstractValidator<DeletePublisherCommand>
    {
        public DeletePublisherCommandValidator()
        {
            RuleFor(c => c.PubisherId).GreaterThan(0).WithErrorCode(ErrorCodeEnum.PublisherIdInvalidValue.ToString());
        }
    }
}
