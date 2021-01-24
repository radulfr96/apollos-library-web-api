﻿using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Common.Exceptions
{
    public class UsernameTakenException : BadRequestException
    {
        public UsernameTakenException(string message = null) : base(ErrorCodeEnum.UsernameAlreadyTaken, message)
        {
        }
    }
}
