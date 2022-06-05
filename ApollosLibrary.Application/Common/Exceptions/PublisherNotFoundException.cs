using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class BusinessNotFoundException : BadRequestException
    {
        public BusinessNotFoundException(string message) : base(ErrorCodeEnum.BusinessNotFound, message)
        {
        }
    }
}
