using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.UserSettings.Queries.GetUserBudgetSettingsQuery
{
    public class GetUserBudgetSettingsQueryDto
    {
        public List<UserBudgetSetting> UserBudgetSettings { get; set; } = new List<UserBudgetSetting>();
    }

    public class UserBudgetSetting
    {
        public int UserBudgetSettingId { get; set; }
        public int Year { get; set; }
        public decimal YearlyBudget { get; set; }
    }
}
