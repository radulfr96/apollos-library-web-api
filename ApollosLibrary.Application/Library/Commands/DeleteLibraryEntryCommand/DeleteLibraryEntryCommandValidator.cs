using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.DeleteLibraryEntryCommand
{
    public class DeleteLibraryEntryCommandValidator : AbstractValidator<DeleteLibraryEntryCommand>
    {
        public DeleteLibraryEntryCommandValidator()
        {
            RuleFor(c => c.LibraryEntryId).GreaterThan(0);
        }
    }
}
