using ApollosLibrary.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Interfaces
{
    public interface IReferenceDataService
    {
        string GetReferenceData();
        Task<List<CountryDTO>> GetCountries();
        Task<BookReferenceDataDTO> GetBookReferenceData();
    }
}
