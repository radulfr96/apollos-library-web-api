using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Interfaces
{
    public class AuthorizationResult
    {
        public bool IsAuthorized { get; }
        public string FailureMessage { get; set; }
        public ErrorCodeEnum ErrorCode { get; set; }

        public AuthorizationResult() { }

        private AuthorizationResult(bool isAuthorized, string failureMessage)
        {
            IsAuthorized = isAuthorized;
            FailureMessage = failureMessage;
        }

        private AuthorizationResult(bool isAuthorized, ErrorCodeEnum errorCode, string failureMessage)
        {
            IsAuthorized = isAuthorized;
            FailureMessage = failureMessage;
            ErrorCode = errorCode;
        }

        public static AuthorizationResult Fail()
        {
            return new AuthorizationResult(false, null);
        }

        public static AuthorizationResult Fail(ErrorCodeEnum errorCode, string failureMessage)
        {
            return new AuthorizationResult(false, errorCode, failureMessage);
        }

        public static AuthorizationResult Succeed()
        {
            return new AuthorizationResult(true, null);
        }
    }

    public interface IAuthorizer<T>
    {
        Task<AuthorizationResult> AuthorizeAsync(T instance, CancellationToken cancellationToken = default);
    }
}
