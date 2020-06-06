using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

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
        void Begin();

        /// <summary>
        /// Used to save data changes
        /// </summary>
        void Save();

        /// <summary>
        /// Used to commit changes
        /// </summary>
        void Commit();
    }
}
