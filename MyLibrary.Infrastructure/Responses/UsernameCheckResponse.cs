using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Infrastructure.Responses
{
    public class UsernameCheckResponse : BaseResponse
    {
        public bool? Result { get; set; }
    }
}
