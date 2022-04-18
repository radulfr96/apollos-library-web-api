using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.UnitOfWork.Contracts
{
    public interface IBusinessUnitOfWork
    {
        /// <summary>
        /// Data layer for Businesss
        /// </summary>
        IBusinessDataLayer BusinessDataLayer { get; }

        /// <summary>
        /// Used to begin a transaction
        /// </summary>
        Task Begin();

        /// <summary>
        /// Used to save data changes
        /// </summary>
        Task Save();

        /// <summary>
        /// Used to commit changes
        /// </summary>
        Task Commit();
    }
}
