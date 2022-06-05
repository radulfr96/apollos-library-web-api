using ApollosLibrary.Application.Moderation.Commands.AddReportEntryCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage moderation
    /// </summary>
    [Route("api/[controller]")]
    public class ModerationController : BaseApiController
    {
        private readonly IMediator _mediatr;

        public ModerationController(IConfiguration config, IMediator mediatr) : base(config)
        {
            _mediatr = mediatr;
        }

        public async Task<AddReportEntryCommandDto> AddEntryReportCommand(AddReportEntryCommand command)
        {
            return await _mediatr.Send(command);
        }
    }
}
