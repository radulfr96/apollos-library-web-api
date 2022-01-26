using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApollosLibrary.UnitOfWork.Contracts
{
    public interface IReferenceUnitOfWork
    {
        /// <summary>
        /// Data layer for reference data
        /// </summary>
        IReferenceDataLayer ReferenceDataLayer { get; }
    }
}
