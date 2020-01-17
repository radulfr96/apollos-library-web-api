using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer;
using MyLibrary.DataLayer.Contracts;
using System;

namespace MyLibrary.UnitOfWork
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private MyLibraryContext _context;

        public UserUnitOfWork(IUserDataLayer userDataLayer)
        {
            UserDataLayer = userDataLayer;
        }

        public IUserDataLayer UserDataLayer { get; }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
