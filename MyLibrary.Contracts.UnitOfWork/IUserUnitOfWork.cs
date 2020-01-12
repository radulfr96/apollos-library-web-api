using MyLibrary.DataLayer.Contracts;
using System;

namespace MyLibrary.Contracts.UnitOfWork
{
    public interface IUserUnitOfWork
    {
        IUserDataLayer UserDataLayer { get; }
        void Commit();
    }
}
