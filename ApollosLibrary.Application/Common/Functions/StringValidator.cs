using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Functions
{
    public static class StringValidator
    {
        public static bool BeValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return Regex.IsMatch(name, "^[A-Za-z -']$");
        }
    }
}
