using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Interfaces
{
    public interface IUserService
    {
        string GetUsername();

        int GetUserId();
    }
}
