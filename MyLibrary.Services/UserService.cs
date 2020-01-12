using MyLibrary.DataLayer.Contracts;
using System;

namespace MyLibrary.Services
{
    public class UserService
    {
        private IUserDataLayer _userDataLayer;

        public UserService(IUserDataLayer userDataLayer)
        {
            _userDataLayer = userDataLayer;
        }
    }
}
