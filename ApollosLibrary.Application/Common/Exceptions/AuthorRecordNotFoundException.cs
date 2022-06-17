using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class AuthorRecordNotFoundException : NotFoundException
    {
        public AuthorRecordNotFoundException(string message) : base(ErrorCodeEnum.AuthorRecordNotFound, message)
        {
        }
    }
}
