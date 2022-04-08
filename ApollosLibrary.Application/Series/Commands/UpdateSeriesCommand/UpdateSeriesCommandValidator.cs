using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.UpdateSeriesCommand
{
    public class UpdateSeriesCommandValidator : AbstractValidator<UpdateSeriesCommand>
    {
        public UpdateSeriesCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithErrorCode(ErrorCodeEnum.SeriesNameNotProvided.ToString());

            RuleFor(c => c.SeriesOrder).ForEach(s =>
            {
                s.Must(s => s.Value > 0).WithErrorCode(ErrorCodeEnum.BookOrderInvalidValue.ToString());
            });
        }
    }
}
