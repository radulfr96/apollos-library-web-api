using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Services.Contracts
{
    public interface IReferenceService
    {
        GetCountriesResponse GetCountries();
    }
}
