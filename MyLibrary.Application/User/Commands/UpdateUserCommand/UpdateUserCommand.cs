using MediatR;
using MyLibrary.Application.Common.DTOs;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Common.Functions;
using MyLibrary.Application.Interfaces;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Persistence.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<UpdateUserCommandDto>
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandDto>
    {
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;
        private readonly Hasher _hasher;
        private readonly ILogger _logger;

        public UpdateUserCommandHandler(IUserUnitOfWork userUnitOfWork, IUserService userService, IDateTimeService dateTimeService, ILogger logger)
        {
            _userUnitOfWork = userUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
            _hasher = new Hasher();
            _logger = logger;
        }

        public async Task<UpdateUserCommandDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateUserCommandDto();

            var userWithUsername = await _userUnitOfWork.UserDataLayer.GetUserByUsername(request.Username);

            if (userWithUsername != null && request.UserID != userWithUsername.UserId)
            {
                throw new UsernameTakenException("Username is already taken");
            }

            var user = await _userUnitOfWork.UserDataLayer.GetUser(request.UserID);

            if (user == null)
            {
                _logger.Warn($"Unable to find as user with id [ {request.UserID} ]");
                 throw new UserNotFoundException("Update unsuccessful user not found");
            }

            user.Username = request.Username;

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.Password = _hasher.HashPassword(request.Password, user.Salter);
            }

            user.ModifiedBy = _userService.GetUserId();
            user.ModifiedDate = _dateTimeService.Now;

            _userUnitOfWork.UserDataLayer.ClearUserRoles(user);

            foreach (var role in request.Roles)
            {
                user.UserRoles.Add(new UserRole()
                {
                    RoleId = role.RoleId,
                    UserId = user.UserId,
                });
            }

            await _userUnitOfWork.Save();

            return response;
        }
    }
}
