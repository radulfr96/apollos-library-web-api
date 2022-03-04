using ApollosLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Filters
{
    public class ErrorCodeTranslation
    {
        private readonly static Dictionary<int, string> _errorCodes = new Dictionary<int, string>();

        public static void Initialise(ApollosLibraryContextOld context)
        {
            if (_errorCodes.Count == 0)
            {
                var errors = (from e in context.ErrorCodes
                              select e).ToList();

                foreach (var e in errors)
                    _errorCodes.Add(e.ErrorCodeId, e.Message);
            }
        }

        public static string  GetErrorMessageFromCode(int errorCodeID)
        {
            _errorCodes.TryGetValue(errorCodeID, out string message);
            return message;
        }
    }
}
