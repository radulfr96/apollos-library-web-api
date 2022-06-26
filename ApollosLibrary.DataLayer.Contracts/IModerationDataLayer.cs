using ApollosLibrary.Domain;
using ApollosLibrary.Domain.DTOs;
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
        /// Used to get users and their counts for reports and reported entries
        /// </summary>
        /// <returns>The user list</returns>
        Task<List<UserDTO>> GetUsers();

        /// <summary>
        /// Used to get an entry report by its id
        /// </summary>
        /// <returns>The entry report to find</returns>
        Task<EntryReport> GetEntryReport(int entryReportid);

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
