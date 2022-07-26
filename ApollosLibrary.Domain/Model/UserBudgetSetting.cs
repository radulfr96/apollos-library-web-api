using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{

    # nullable disable

    public class UserBudgetSetting
    {
        [Key]
        public int UserBudgetSettingId { get; set; }
        public int Year { get; set; }
        public decimal YearlyBudget { get; set; }
        public Guid UserId { get; set; }
    }
}
