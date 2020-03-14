using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.UnitOfWork.Contracts
{
    /// <summary>
    /// Used to perform units of work for genres
    /// </summary>
    public interface IGenreUnitOfWork
    {
        /// <summary>
        /// Data layer for genres
        /// </summary>
        IGenreDataLayer GenreDataLayer { get; }

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
