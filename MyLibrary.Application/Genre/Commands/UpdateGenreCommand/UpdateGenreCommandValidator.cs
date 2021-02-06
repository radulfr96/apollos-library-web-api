using FluentValidation;
using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Commands.UpdateGenreCommand
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty().WithErrorCode(ErrorCodeEnum.GenreNameNotProvided.ToString());
            RuleFor(g => g.Name).Length(1, 50).WithErrorCode(ErrorCodeEnum.GenreNameInvalidLength.ToString());
        }
    }
}
