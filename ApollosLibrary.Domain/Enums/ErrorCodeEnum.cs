using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain.Enums
{
    public enum ErrorCodeEnum
    {
        SystemError = 0,

        // Validation Errors
        CountryInvalidValue = 1,
        ISBNAlreadyAdded = 2,
        WebsiteInvalidLength = 3,
        BusinessIsNotABookshop = 4,
        BusinessIsNotAPublisher = 5,
        StripeSubscriptionMissingUserId = 6,

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
        LibraryEntryNotFound = 111,
        OrderNotFound = 112,
        SubscriptionNotFound = 113,
        SubscriptionTypeNotFound = 114,
        EntryReportNotFound = 115,

        // User Forbidden Exception
        UserCannotModifyLibrary = 200,
        UserCannotAccessOrder = 201,
    }
}
