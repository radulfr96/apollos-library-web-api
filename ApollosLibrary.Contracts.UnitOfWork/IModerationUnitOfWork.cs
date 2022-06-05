using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.UnitOfWork.Contracts
{
    public interface IModerationUnitOfWork
    {
        /// <summary>
        /// Data layer for moderation data
        /// </summary>
        IModerationDataLayer ModerationDataLayer { get; }

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
