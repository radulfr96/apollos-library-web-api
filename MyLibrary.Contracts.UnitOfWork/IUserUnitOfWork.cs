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
        /// Data layer for roles
        /// </summary>
        IRoleDataLayer RoleDataLayer { get; }

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
