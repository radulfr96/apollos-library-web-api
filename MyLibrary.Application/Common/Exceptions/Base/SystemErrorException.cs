using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Common.Exceptions
{
    public class SystemErrorException : ErrorCodeException
    {
        public SystemErrorException(string message = null)
        : base(ErrorCodeEnum.SystemError, message)
        {
        }
    }
}
