using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.UnitOfWork.Contracts
{
    public interface IAuthorUnitOfWork
    {
        /// <summary>
        /// Data layer for authors
        /// </summary>
        IAuthorDataLayer AuthorDataLayer { get; }

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
