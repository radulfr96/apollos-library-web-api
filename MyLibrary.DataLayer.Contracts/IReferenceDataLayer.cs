using MyLibrary.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.DataLayer.Contracts
{
    public interface IReferenceDataLayer
    {
        List<Country> GetCountries();
    }
}
