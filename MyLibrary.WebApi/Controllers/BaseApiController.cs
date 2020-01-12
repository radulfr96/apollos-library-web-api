using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Common.Responses;
using NLog;

namespace MyLibrary.WebApi.Controllers
{
    /// <summary>
    /// This is used as the base for all of the api controllers as all them will need
    /// the methods and attributes contained within this object
    /// </summary>
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected static Logger s_logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Used to generate a readable and formatted bad request message to return to the user
        /// </summary>
        /// <param name="response">The response with the bad request messages</param>
        /// <returns></returns>
        protected string BuildBadRequestMessage(BaseResponse response)
        {
            string message = string.Empty;
            try
            {
                foreach (string statusMessage in response.Messages)
                    message += statusMessage + "\n";
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to build bad request message.");
                message = string.Empty;
            }

            return message;
        }
    }
}