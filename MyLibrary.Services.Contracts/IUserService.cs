using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using System;

namespace MyLibrary.Services.Contracts
{
    public interface IUserService
    {
        public GetUsersResponse GetUsers();

        public LoginResponse Login(LoginRequest request);
    }
}
