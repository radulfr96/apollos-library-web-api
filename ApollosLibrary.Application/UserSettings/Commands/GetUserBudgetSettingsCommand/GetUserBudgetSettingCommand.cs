using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.UserSettings.Commands.GetUserBudgetSettingsCommand
{
    public class GetUserBudgetSettingCommand : IRequest<GetUserBudgetSettingCommandDto>
    {
    }

    public class GetUserBudgetSettingsCommandHandler : IRequestHandler<GetUserBudgetSettingCommand, GetUserBudgetSettingCommandDto>
    {
        private readonly IUserService _userService;
        private readonly IUserSettingsUnitOfWork _userSettingsUnitOfWork;
        private readonly IDateTimeService _dateTimeService;

        public GetUserBudgetSettingsCommandHandler(
            IUserService userService
            , IUserSettingsUnitOfWork userSettingsUnitOfWork
            , IDateTimeService dateTimeService
            )
        {
            _userService = userService;
            _userSettingsUnitOfWork = userSettingsUnitOfWork;
            _dateTimeService = dateTimeService;
        }

        public async Task<GetUserBudgetSettingCommandDto> Handle(GetUserBudgetSettingCommand command, CancellationToken cancellationToken)
        {
            int year = _dateTimeService.Now.Year;

            var setting = await _userSettingsUnitOfWork.UserSettingsDataLayer.GetUserBudgetSetting(_userService.GetUserId(), year);

            if (setting == null)
            {
                setting = new Domain.UserBudgetSetting()
                {
                    UserId = _userService.GetUserId(),
                    Year = _dateTimeService.Now.Year,
                    YearlyBudget = 0.00m,
                };

                await _userSettingsUnitOfWork.UserSettingsDataLayer.AddUserBudgetSetting(setting);
                await _userSettingsUnitOfWork.Save();
            }

            return new GetUserBudgetSettingCommandDto()
            {
                UserBudgetSettingId = setting.UserBudgetSettingId,
                Year = setting.Year,
                YearlyBudget = setting.YearlyBudget,
            };
        }
    }
}
