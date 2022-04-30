using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand
{
    public class AddLibraryEntryCommandValidator : AbstractValidator<AddLibraryEntryCommand>
    {
        public AddLibraryEntryCommandValidator()
        {
            RuleFor(c => c.BookId).GreaterThan(0);
            RuleFor(c => c.LibraryId).GreaterThan(0);
            RuleFor(c => c.Quantity).GreaterThan(0);
        }
    }
}
