using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class BusinessTypeNotFoundException : NotFoundException
    {
        public BusinessTypeNotFoundException(string message, Exception inner = null) : base(ErrorCodeEnum.BusinessTypeNotFound, message, inner)
        {
        }
    }
}
