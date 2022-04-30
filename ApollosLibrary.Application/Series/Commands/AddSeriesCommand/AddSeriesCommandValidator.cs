using ApollosLibrary.Application.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.AddSeriesCommand
{
    public class AddSeriesCommandValidator : AbstractValidator<AddSeriesCommand>
    {
        public AddSeriesCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
