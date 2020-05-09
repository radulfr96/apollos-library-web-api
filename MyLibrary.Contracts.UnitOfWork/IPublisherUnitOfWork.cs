using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.UnitOfWork.Contracts
{
    public interface IPublisherUnitOfWork
    {
        /// <summary>
        /// Data layer for publishers
        /// </summary>
        IPublisherDataLayer PublisherDataLayer { get; }

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
