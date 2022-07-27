using ApollosLibrary.Application.UserSettings.Commands.GetUserBudgetSettingsCommand;
using ApollosLibrary.Application.UserSettings.Queries.GetUserBudgetSettingsQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage a users settings
    /// </summary>
    [Route("api/[controller]")]
    public class UserSettingsController : BaseApiController
    {
        private readonly IMediator _mediatr;
        public UserSettingsController(IConfiguration config, IMediator mediatr) : base(config)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Used to get user budget settings
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("budgetsettings")]
        public async Task<GetUserBudgetSettingsQueryDto> GetUserBudgetSettings()
        {
            return await _mediatr.Send(new GetUserBudgetSettingsQuery());
        }

        /// <summary>
        /// Used to get user budget setting
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetUserBudgetSettingCommandDto> GetUserBudgetSetting()
        {
            return await _mediatr.Send(new GetUserBudgetSettingCommand());
        }
    }
}
