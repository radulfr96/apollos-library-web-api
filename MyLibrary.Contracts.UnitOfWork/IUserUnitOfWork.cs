using MyLibrary.DataLayer.Contracts;
using System;

namespace MyLibrary.Contracts.UnitOfWork
{
    /// <summary>
    /// Used to perform units of work for users
    /// </summary>
    public interface IUserUnitOfWork
    {
        /// <summary>
        /// Data layer for users
        /// </summary>
        IUserDataLayer UserDataLayer { get; }

        /// <summary>
        /// Used to commit changes
        /// </summary>
        void Commit();
    }
}
