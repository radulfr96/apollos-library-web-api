using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {

        public IList<string> Errors { get; }

        public ValidationException() : base ("One or more validation failures have occured")
        {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
        {
            Errors = failures.Select(e => e.ErrorMessage).ToList();
        }
    }
}
