using FluentValidation;
using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Commands.AddBookCommand
{
    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator()
        {
            RuleFor(b => b.Title).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());

            When(b => string.IsNullOrEmpty(b.ISBN), () =>
            {
                RuleFor(b => b.eISBN).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
            });


        }
    }
}
