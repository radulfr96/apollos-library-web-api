using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class NotFoundException : ErrorCodeException
    {
        public NotFoundException(ErrorCodeEnum errorCode, string message = null, Exception inner = null)
        : base(errorCode ,message, inner)
        {
        }
    }
}
