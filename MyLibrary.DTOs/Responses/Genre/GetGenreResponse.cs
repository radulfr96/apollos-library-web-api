using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetGenreResponse : BaseResponse
    {
        public GenreDTO Genre { get; set; }
    }
}
