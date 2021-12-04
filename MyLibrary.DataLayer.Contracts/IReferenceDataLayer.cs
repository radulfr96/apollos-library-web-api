using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer.Contracts
{
    public interface IReferenceDataLayer
    {
        Task<List<Country>> GetCountries();

        Task<PublicationFormat> GetPublicationFormat(int publicationFormatId);

        Task<FormType> GetFormType(int formTypeId);

        Task<FictionType> GetFictionType(int fictionTypeId);
    }
}
