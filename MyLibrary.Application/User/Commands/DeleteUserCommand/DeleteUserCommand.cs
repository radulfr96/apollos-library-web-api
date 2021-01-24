using MediatR;
using MyLibrary.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : IRequest<DeleteUserCommandDto>
    {
        public int UserId { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserCommandDto>
    {
        private readonly IUserUnitOfWork _userUnitOfWork;

        public DeleteUserCommandHandler(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task<DeleteUserCommandDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteUserCommandDto();

            var user = await _userUnitOfWork.UserDataLayer.GetUser(id);

            if (user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Messages.Add($"Unable to delete user with id [{id}]");
                return response;
            }

            user.IsDeleted = true;
            user.Username = "Deleted";

            await _userUnitOfWork.Save();

            return response;
        }
    }
}
