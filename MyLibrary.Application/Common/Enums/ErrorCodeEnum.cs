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

        // Update Author
        AuthorNotFound = 100,

        BookNotFound = 200,
        ISBNAlreadyAdded = 201,

        GenreNotFound = 300,

        PublisherNotFound = 400,

        UserNotFound = 500,
        UsernameAlreadyTaken = 501,
    }
}
