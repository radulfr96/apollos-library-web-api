using FluentValidation;
using MyLibrary.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Commands.UpdateBookCommand
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(b => b.Title).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());

            When(b => string.IsNullOrEmpty(b.ISBN), () =>
            {
                RuleFor(b => b.eISBN).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            });
        }
    }
}
