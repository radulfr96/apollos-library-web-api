using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Enums
{
    public enum ErrorCodeEnum
    {
        SystemError = 0,

        // Validation Errors
        CountryInvalidValue = 10,
        ISBNAlreadyAdded = 13,
        WebsiteInvalidLength = 28,

        // Not Found Errors
        AuthorNotFound = 100,
        BookNotFound = 101,
        GenreNotFound = 102,
        BusinessNotFound = 103,
        UserNotFound = 104,
        SeriesNotFound = 105,
        PublicationFormatNotFound = 106,
        FictionTypeNotFound = 107,
        FormTypeNotFound = 108,
        BusinessTypeNotFound = 109,
        LibraryNotFound = 110,

        // User Forbidden Exception
        UserCannotModifyLibrary = 200,
    }
}
