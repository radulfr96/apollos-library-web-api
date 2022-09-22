using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.UserSettings.Queries.GetUserBudgetProgressQuery
{
    public class GetUserBudgetProgressQueryResponse
    {
        public decimal MontlhyBudget { get; set; }
        public List<Amount> Amounts { get; set; } = new List<Amount>()
        {
            new Amount()
            {
                Month = 1,
            },
            new Amount()
            {
                Month = 2,
            },
            new Amount()
            {
                Month = 3,
            },
            new Amount()
            {
                Month = 4,
            },
            new Amount()
            {
                Month = 5,
            },
            new Amount()
            {
                Month = 6,
            },
            new Amount()
            {
                Month = 7,
            },
            new Amount()
            {
                Month = 8,
            },
            new Amount()
            {
                Month = 9,
            },
            new Amount()
            {
                Month = 10,
            },
            new Amount()
            {
                Month = 11,
            },
            new Amount()
            {
                Month = 12,
            },
        };
    }

    public class Amount
    {
        public int Month { get; set; }
        public decimal Spend { get; set; }
    }
}
