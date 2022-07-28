using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    public interface IUserSettingsDataLayer
    {
        /// <summary>
        /// Returns the users budget settins, past and present
        /// </summary>
        /// <param name="userId">The id whose settings need to be returned</param>
        /// <returns>The users budget settinfs</returns>
        Task<List<UserBudgetSetting>> GetUserBudgetSettings(Guid userId);

        /// <summary>
        /// Used to get the users settings 
        /// </summary>
        /// <param name="userId">The id of the users whose settings need to be returned</param>
        /// <param name="year">The year for the settings desired</param>
        /// <returns>The settings for the user and year</returns>
        Task<UserBudgetSetting> GetUserBudgetSetting(Guid userId, int year);

        /// <summary>
        /// Used to add a user setting
        /// </summary>
        /// <param name="setting">The new setting to be saved</param>
        Task AddUserBudgetSetting(UserBudgetSetting setting);
    }
}
