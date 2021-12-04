﻿using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Common.Exceptions
{
    public class FormTypeNotFoundException : NotFoundException
    {
        public FormTypeNotFoundException(string message) : base(ErrorCodeEnum.FormTypeNotFound, message)
        {

        }
    }
}
