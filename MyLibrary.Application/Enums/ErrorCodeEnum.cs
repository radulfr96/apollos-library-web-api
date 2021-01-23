using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Enums
{
    public enum ErrorCodeEnum
    {
        SystemError = 0,

        // AddAuthorErrors
        FirstnameNotProvided = 1,
        LastnameNotProvided = 2,
        CountryNotProvided = 3,
    }
}
