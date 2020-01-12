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
        private IUserDataLayer _userDataLayer;

        public UserUnitOfWork(MyLibraryContext context)
        {
            _context = context;
        }

        public IUserDataLayer UserDataLayer
        {
            get
            {
                if (_userDataLayer == null)
                {
                    _userDataLayer = new UserDataLayer(_context);
                }
                return _userDataLayer;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
