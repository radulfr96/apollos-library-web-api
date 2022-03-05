using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.Exceptions
{
    public class PasswordInvalidException : BadRequestException
    {
        public PasswordInvalidException(string message) : base(message) { }
    }
}
