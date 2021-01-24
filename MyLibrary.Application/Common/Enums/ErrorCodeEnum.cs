using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Common.Enums
{
    public enum ErrorCodeEnum
    {
        SystemError = 0,

        // AddAuthorErrors
        FirstnameNotProvided = 1,
        LastnameNotProvided = 2,
        CountryNotProvided = 3,


        BookNotFound = 100,
        ISBNAlreadyAdded = 101,

        GenreNotFound = 200,

        PublisherNotFound = 300,

        UserNotFound = 400,
        UsernameAlreadyTaken = 401,
    }
}
