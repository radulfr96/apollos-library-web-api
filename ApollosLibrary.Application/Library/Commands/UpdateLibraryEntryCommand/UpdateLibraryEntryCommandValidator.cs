using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.UpdateLibraryEntryCommand
{
    public class UpdateLibraryEntryCommandValidator : AbstractValidator<UpdateLibraryEntryCommand>
    {
        public UpdateLibraryEntryCommandValidator()
        {
            RuleFor(c => c.LibraryEntryId).GreaterThan(0);
            RuleFor(c => c.Quantity).GreaterThan(0);
        }
    }
}
