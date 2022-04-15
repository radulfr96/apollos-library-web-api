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
            RuleFor(c => c.SeriesId).GreaterThan(0).WithErrorCode(ErrorCodeEnum.SeriesIdInvalidValue.ToString());

            RuleFor(c => c.Name).NotEmpty().WithErrorCode(ErrorCodeEnum.SeriesNameNotProvided.ToString());
        }
    }
}
