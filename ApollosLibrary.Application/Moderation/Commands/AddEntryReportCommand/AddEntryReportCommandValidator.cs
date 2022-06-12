using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Commands.AddEntryReportCommand
{
    public class AddEntryReportCommandValidator : AbstractValidator<AddEntryReportCommand>
    {
        public AddEntryReportCommandValidator()
        {
            RuleFor(r => r.EntryId).GreaterThan(0);
        }
    }
}
