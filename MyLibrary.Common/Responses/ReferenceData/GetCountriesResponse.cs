using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetCountriesResponse : BaseResponse
    {
        public List<CountryDTO> Countries { get; set; }

        public GetCountriesResponse()
        {
            Countries = new List<CountryDTO>();
        }
    }
}
