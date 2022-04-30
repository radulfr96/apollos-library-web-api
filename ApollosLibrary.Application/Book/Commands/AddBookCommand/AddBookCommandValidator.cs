using FluentValidation;
using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Commands.AddBookCommand
{
    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator()
        {
            When(b => !string.IsNullOrEmpty(b.ISBN), () =>
            {
                RuleFor(b => b.ISBN).Length(10, 13);
                RuleFor(b => b.ISBN).Must(BeValidISBN);
            });

            When(b => !string.IsNullOrEmpty(b.EISBN), () =>
            {
                RuleFor(b => b.EISBN).Length(10, 13);
                RuleFor(b => b.EISBN).Must(BeValidISBN);
            });

            RuleFor(b => b.Title).NotEmpty();
            RuleFor(b => b.Title).Length(1, 200);

            When(b => !string.IsNullOrEmpty(b.Subtitle), () =>
            {
                RuleFor(b => b.Subtitle).Length(1, 200);
            });

            RuleFor(b => b.Edition).GreaterThan(0);
        }

        private bool BeValidISBN(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return false;

            return Regex.IsMatch(isbn, "^[0-9]$");
        }
    }
}
