using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.UserSettings.Commands.GetUserBudgetSettingsCommand
{
    public class GetUserBudgetSettingCommandDto
    {
        public int UserBudgetSettingId { get; set; }
        public int Year { get; set; }
        public decimal YearlyBudget { get; set; }
    }
}
