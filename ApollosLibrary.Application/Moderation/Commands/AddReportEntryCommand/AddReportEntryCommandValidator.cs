using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Commands.AddReportEntryCommand
{
    public class AddReportEntryCommandValidator : AbstractValidator<AddReportEntryCommand>
    {
        public AddReportEntryCommandValidator()
        {
            RuleFor(r => r.EntryId).GreaterThan(0);
        }
    }
}
