using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Common.Exceptions
{
    public class UnauthorizedException : ErrorCodeException
    {
        public UnauthorizedException(ErrorCodeEnum errorCode, string message = null, Exception inner = null)
        : base(errorCode, message, inner)
        {
        }
    }
}
