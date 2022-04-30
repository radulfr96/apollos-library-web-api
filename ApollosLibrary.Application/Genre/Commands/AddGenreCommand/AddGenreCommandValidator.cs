using FluentValidation;
using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Genre.Commands.AddGenreCommand
{
    public class AddGenreCommandValidator : AbstractValidator<AddGenreCommand>
    {
        public AddGenreCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty();
            RuleFor(g => g.Name).Length(1, 50);
        }
    }
}
