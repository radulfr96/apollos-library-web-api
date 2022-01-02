using MediatR;
using Microsoft.Extensions.Logging;
using MyLibrary.Application.Common.DTOs;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Common.Functions;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Commands.UpdateSelfUserCommand
{
    public class UpdateSelfUserCommand : IRequest<UpdateSelfUserCommandDto>
    {
        public string Username { get; set; }
    }

    public class UpdateSelfUserCommandHandler : IRequestHandler<UpdateSelfUserCommand, UpdateSelfUserCommandDto>
    {
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger _logger;

        public UpdateSelfUserCommandHandler(IUserUnitOfWork userUnitOfWork, IUserService userService, IDateTimeService dateTimeService, ILogger<UpdateSelfUserCommandHandler> logger)
        {
            _userUnitOfWork = userUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public async Task<UpdateSelfUserCommandDto> Handle(UpdateSelfUserCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateSelfUserCommandDto();

            var userWithUsername = await _userUnitOfWork.UserDataLayer.GetUserByUsername(request.Username);

            if (userWithUsername != null && _userService.GetUserId() != userWithUsername.UserId)
            {
                throw new UsernameTakenException("Username is already taken");
            }

            var user = await _userUnitOfWork.UserDataLayer.GetUser(_userService.GetUserId());

            if (user == null)
            {
                _logger.LogWarning($"Unable to find as user with id [ {_userService.GetUserId()} ]");
                throw new UserNotFoundException("Update unsuccessful user not found");
            }

            user.Username = request.Username;

            user.ModifiedBy = _userService.GetUserId();
            user.ModifiedDate = _dateTimeService.Now;

            await _userUnitOfWork.Save();

            return response;
        }
    }
}
