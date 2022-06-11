using ApollosLibrary.Domain.Enums;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class AuthorNotFoundException : NotFoundException
    {
        public AuthorNotFoundException(string message) : base(ErrorCodeEnum.AuthorNotFound, message)
        {
        }
    }
}
