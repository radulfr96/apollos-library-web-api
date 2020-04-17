using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetAuthorResponse : BaseResponse
    {
        public AuthorDTO Author { get; set; }
    }
}
