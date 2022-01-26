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

            When(b => string.IsNullOrEmpty(b.ISBN), () =>
            {
                RuleFor(b => b.EISBN).NotEmpty().WithErrorCode(ErrorCodeEnum.NoISBNOreISBNProvided.ToString());
            });

            When(b => !string.IsNullOrEmpty(b.ISBN), () =>
            {
                RuleFor(b => b.ISBN).Length(10, 13).WithErrorCode(ErrorCodeEnum.ISBNInvalidLength.ToString());
                RuleFor(b => b.ISBN).Must(BeValidISBN).WithErrorCode(ErrorCodeEnum.ISBNInvalidLength.ToString());
            });

            When(b => !string.IsNullOrEmpty(b.EISBN), () =>
            {
                RuleFor(b => b.EISBN).Length(10, 13).WithErrorCode(ErrorCodeEnum.eISBNInvalidLength.ToString());
                RuleFor(b => b.EISBN).Must(BeValidISBN).WithErrorCode(ErrorCodeEnum.eISBNInvalidFormat.ToString());
            });

            RuleFor(b => b.Title).NotEmpty().WithErrorCode(ErrorCodeEnum.TitleNotProvided.ToString());
            RuleFor(b => b.Title).Length(1, 200).WithErrorCode(ErrorCodeEnum.TitleInvalidLength.ToString());

            When(b => !string.IsNullOrEmpty(b.Subtitle), () =>
            {
                RuleFor(b => b.Subtitle).Length(1, 200).WithErrorCode(ErrorCodeEnum.SubtitleInvalidLength.ToString());
            });

            RuleFor(b => b.NumberInSeries).GreaterThanOrEqualTo(0).WithErrorCode(ErrorCodeEnum.NumberInSeriesInvalidValue.ToString());

            RuleFor(b => b.Edition).GreaterThan(0).WithErrorCode(ErrorCodeEnum.EditionInvalidValue.ToString());
        }

        private bool BeValidISBN(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return false;

            return Regex.IsMatch(isbn, "^[0-9]$");
        }
    }
}
