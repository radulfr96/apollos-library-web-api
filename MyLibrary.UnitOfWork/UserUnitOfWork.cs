using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.DataLayer.Contracts;

namespace MyLibrary.UnitOfWork
{
    public class UserUnitOfWork : IUserUnitOfWork
    {

        public UserUnitOfWork(IUserDataLayer userDataLayer, IRoleDataLayer roleDataLayer)
        {
            UserDataLayer = userDataLayer;
            RoleDataLayer = roleDataLayer;
        }

        public IUserDataLayer UserDataLayer { get; }

        public IRoleDataLayer RoleDataLayer { get; }

        public void Commit()
        {
            UserDataLayer.Save();
            RoleDataLayer.Save();
        }
    }
}
