using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.UnitOfWork.Contracts
{
    public interface IBookUnitOfWork
    {
        /// <summary>
        /// Data layer for books
        /// </summary>
        IBookDataLayer BookDataLayer { get; }

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

        /// <summary>
        /// Used to reverse changes in a transaction
        /// </summary>
        Task Rollback();
    }
}
