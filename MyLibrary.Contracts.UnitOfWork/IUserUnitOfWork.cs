using MyLibrary.DataLayer.Contracts;
using System;

namespace MyLibrary.Contracts.UnitOfWork
{
    public interface IUserUnitOfWork
    {
        IUserDataLayer UserDataLayer { get; }
        public IRoleDataLayer RoleDataLayer { get; }
        void Commit();
    }
}
