
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
            RuleFor(c => c.SeriesId).GreaterThan(0);

            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
