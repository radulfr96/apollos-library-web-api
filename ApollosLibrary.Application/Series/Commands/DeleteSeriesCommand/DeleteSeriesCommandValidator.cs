using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.DeleteSeriesCommand
{
    public class DeleteSeriesCommandValidator : AbstractValidator<DeleteSeriesCommand>
    {
        public DeleteSeriesCommandValidator()
        {
            RuleFor(c => c.SeriesId).GreaterThan(0);
        }
    }
}
