using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class LibraryNotFoundException : NotFoundException
    {
        public LibraryNotFoundException(string message) : base(ErrorCodeEnum.LibraryNotFound, message)
        {
        }
    }
}
