using ApollosLibrary.Domain.Enums;
using System;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class BadRequestException : ErrorCodeException
    {
        public BadRequestException(ErrorCodeEnum errorCode, string message = null, Exception inner = null) : base(errorCode, message, inner)
        {
        }
    }
}
