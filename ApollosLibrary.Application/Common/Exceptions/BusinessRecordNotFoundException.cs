using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class BusinessRecordNotFoundException : NotFoundException
    {
        public BusinessRecordNotFoundException(string message) : base(ErrorCodeEnum.BusinessRecordNotFound, message)
        {
        }
    }
}
