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
        FirstnameInvalidLength = 2,
        FirstnameInvalidFormat = 3,
        MiddlenameInvalidLength = 4,
        MiddlenameInvalidFormat = 5,
        LastnameNotProvided = 6,
        LastnameInvalidLength = 7,
        LastnameInvalidFormat = 8,
        CountryNotProvided = 9,
        CountryInvalidValue = 10,
        DecriptionInvalidLength = 11,

        BookNotFound = 100,
        ISBNAlreadyAdded = 101,

        GenreNotFound = 200,

        PublisherNotFound = 300,

        UserNotFound = 400,
        UsernameAlreadyTaken = 401,
    }
}
