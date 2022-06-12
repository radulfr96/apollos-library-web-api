using ApollosLibrary.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Commands.UpdateEntryReportCommand
{
    public class UpdateEntryReportCommandValidator : AbstractValidator<UpdateEntryReportCommand>
    {
        public UpdateEntryReportCommandValidator()
        {
            RuleFor(c => c.EntryReportId).GreaterThan(0);
            RuleFor(c => c.EntryReportStatus).IsInEnum();
        }
    }
}
