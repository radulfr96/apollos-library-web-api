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
        NoISBNOreISBNProvided = 12,
        ISBNAlreadyAdded = 13,
        ISBNInvalidLength = 14,
        ISBNInvalidFormat = 15,
        eISBNAlreadyAdded = 16,
        eISBNInvalidLength = 17,
        eISBNInvalidFormat = 18,
        TitleNotProvided = 19,
        TitleInvalidLength = 20,
        SubtitleInvalidLength = 21,
        NumberInSeriesInvalidValue = 22,
        EditionInvalidValue = 23,
        GenreNameNotProvided = 24,
        GenreNameInvalidLength = 25,
        PublisherNameNotProvided = 26,
        PublisherNameInvalidLength = 27,
        WebsiteInvalidLength = 28,
        InvalidAddressProvided = 29,
        PasswordInvalid = 30,
        AuthorIdInvalidValue = 31,
        BookIdInvalidValue = 32,
        GenreIdInvalidValue = 33,
        PublisherIdInvalidValue = 34,
        SeriesIdInvalidValue = 35,
        SeriesNameNotProvided = 36,
        SeriesNameInvalidLength = 38,
        BookOrderInvalidValue = 39,

        // Not Found Errors
        AuthorNotFound = 100,
        BookNotFound = 101,
        GenreNotFound = 102,
        PublisherNotFound = 103,
        UserNotFound = 104,
        SeriesNotFound = 105,
        PublicationFormatNotFound = 106,
        FictionTypeNotFound = 107,
        FormTypeNotFound = 108,

    }
}
