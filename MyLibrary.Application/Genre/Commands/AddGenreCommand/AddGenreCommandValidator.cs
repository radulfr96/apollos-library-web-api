﻿using FluentValidation;
using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Commands.AddGenreCommand
{
    public class AddGenreCommandValidator : AbstractValidator<AddGenreCommand>
    {
        public AddGenreCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty().WithErrorCode(ErrorCodeEnum.FirstnameNotProvided.ToString());
        }
    }
}
