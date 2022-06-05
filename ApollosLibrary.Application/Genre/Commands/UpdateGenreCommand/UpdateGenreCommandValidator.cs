using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Genre.Commands.UpdateGenreCommand
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty();
            RuleFor(g => g.Name).Length(1, 50);
        }
    }
}
