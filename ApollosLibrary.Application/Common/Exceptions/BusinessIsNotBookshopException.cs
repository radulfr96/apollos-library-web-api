﻿using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    internal class BusinessIsNotBookshopException : BadRequestException
    {
        public BusinessIsNotBookshopException(string message = null) : base(ErrorCodeEnum.BusinessIsNotABookshop, message)
        {
        }
    }
}