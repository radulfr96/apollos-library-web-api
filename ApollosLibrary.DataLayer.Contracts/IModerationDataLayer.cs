using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Data layer used for handling moderation data
    /// </summary>
    public interface IModerationDataLayer
    {
        /// <summary>
        /// Used to add an entry report
        /// </summary>
        /// <param name="entryReport">The report</param>
        Task AddEntryReport(EntryReport entryReport);

        /// <summary>
        /// Used to get all of the entry reports
        /// </summary>
        /// <returns>The entry reports</returns>
        Task<List<EntryReport>> GetEntryReports();

        /// <summary>
        /// Used to get all of the entry reports created by a user
        /// </summary>
        /// <returns>The entry reports</returns>
        Task<List<EntryReport>> GetUsersEntryReports(Guid userId);

        /// <summary>
        /// Used to get all of the entry reports where the entity was created by user
        /// </summary>
        /// <returns>The entry reports</returns>
        Task<List<EntryReport>> GetReportsOfEntriesByUser(Guid userId);
    }
}
