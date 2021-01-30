using MyLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Common.Exceptions
{
    public class PublisherNotFoundException : BadRequestException
    {
        public PublisherNotFoundException(string message) : base(ErrorCodeEnum.PublisherNotFound, message)
        {
        }
    }
}
