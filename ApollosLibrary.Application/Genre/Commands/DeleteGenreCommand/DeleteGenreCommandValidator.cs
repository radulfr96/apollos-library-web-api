
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Genre.Commands.DeleteGenreCommand
{
    public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
    {
        public DeleteGenreCommandValidator()
        {
            RuleFor(c => c.GenreId).GreaterThan(0);
        }
    }
}
