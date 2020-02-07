using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.DataLayer.Contracts;

namespace MyLibrary.UnitOfWork
{
    public class UserUnitOfWork : IUserUnitOfWork
    {

        public UserUnitOfWork(IUserDataLayer userDataLayer)
        {
            UserDataLayer = userDataLayer;
        }

        public IUserDataLayer UserDataLayer { get; }

        public void Commit()
        {
            UserDataLayer.Save();
        }
    }
}
