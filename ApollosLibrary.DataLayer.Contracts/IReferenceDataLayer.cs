﻿using ApollosLibrary.Domain;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    public interface IReferenceDataLayer
    {
        Task<List<Country>> GetCountries();
        Task<List<BusinessType>> GetBusinessTypes();
        Task<PublicationFormat> GetPublicationFormat(int publicationFormatId);
        Task<FormType> GetFormType(int formTypeId);
        Task<FictionType> GetFictionType(int fictionTypeId);
        Task<List<PublicationFormat>> GetPublicationFormats();
        Task<List<FormType>> GetFormTypes();
        Task<List<FictionType>> GetFictionTypes();
    }
}
