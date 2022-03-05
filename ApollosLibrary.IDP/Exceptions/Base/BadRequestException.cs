using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = null, Exception inner = null)
        : base(message, inner)
        {
        }
    }
}
