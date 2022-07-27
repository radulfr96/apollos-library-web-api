using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.UserSettings.Queries.GetUserBudgetSettingsQuery
{
    public class GetUserBudgetSettingsQuery : IRequest<GetUserBudgetSettingsQueryDto>
    {
    }

    public class GetUserBudgetSettingsQueryHandler : IRequestHandler<GetUserBudgetSettingsQuery, GetUserBudgetSettingsQueryDto>
    {
        private readonly IUserService _userService;
        private readonly IUserSettingsUnitOfWork _userSettingsUnitOfWork;

        public GetUserBudgetSettingsQueryHandler(
            IUserService userService
            , IUserSettingsUnitOfWork userSettingsUnitOfWork
            )
        {
            _userService = userService;
            _userSettingsUnitOfWork = userSettingsUnitOfWork;
        }

        public async Task<GetUserBudgetSettingsQueryDto> Handle(GetUserBudgetSettingsQuery query, CancellationToken cancellationToken)
        {
            var settings = await _userSettingsUnitOfWork.UserSettingsDataLayer.GetUserBudgetSettings(_userService.GetUserId());

            return new GetUserBudgetSettingsQueryDto()
            {
                UserBudgetSettings = settings.Select(u => new UserBudgetSetting()
                {
                    UserBudgetSettingId = u.UserBudgetSettingId,
                    Year = u.Year,
                    YearlyBudget = u.YearlyBudget,
                }).ToList(),
            };
        }
    }
}
