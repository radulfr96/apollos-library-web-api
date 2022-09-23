using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Dashboard.Queries.GetUserBudgetProgressQuery
{
    public class GetUserBudgetReportQuery : IRequest<GetUserBudgetReportQueryResponse>
    {
        public int Year { get; set; }
    }

    public class GetUserBudgetProgressQueryHandler : IRequestHandler<GetUserBudgetReportQuery, GetUserBudgetReportQueryResponse>
    {
        private readonly IUserService _userService;
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IUserSettingsUnitOfWork _userSettingsUnitOfWork;

        public GetUserBudgetProgressQueryHandler(
            IUserService userService
            , IOrderUnitOfWork orderUnitOfWork
            , IUserSettingsUnitOfWork userSettingsUnitOfWork
            )
        {
            _userService = userService;
            _orderUnitOfWork = orderUnitOfWork;
            _userSettingsUnitOfWork = userSettingsUnitOfWork;
        }

        public async Task<GetUserBudgetReportQueryResponse> Handle(GetUserBudgetReportQuery request, CancellationToken cancellationToken)
        {
            var yearlyBudget = await _userSettingsUnitOfWork.UserSettingsDataLayer
                                    .GetUserBudgetSetting(_userService.GetUserId(), request.Year);

            var response = new GetUserBudgetReportQueryResponse()
            {
                MontlhyBudget = yearlyBudget == null ? 0 : Math.Round(yearlyBudget.YearlyBudget / 12, 2),
            };

            var orders = await _orderUnitOfWork.OrderDataLayer.GetOrdersByYear(_userService.GetUserId(), request.Year);

            foreach (var amount in response.Amounts)
            {
                var itemsForMonth = orders.Where(o => o.OrderDate.Month == amount.Month).SelectMany(o => o.OrderItems).ToList();

                amount.Spend = itemsForMonth.Select(i => i.Price * i.Quantity).Sum();
            }

            return response;
        }
    }
}
