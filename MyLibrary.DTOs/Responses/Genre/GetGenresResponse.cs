using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetGenresResponse : BaseResponse
    {
        public List<GenreDTO> Genres { get; set; }
    }
}
