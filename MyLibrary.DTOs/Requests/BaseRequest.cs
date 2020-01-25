using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class BaseRequest : IValidatableObject
    {
        public BaseResponse ValidateRequest(BaseResponse response)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            if (!isValid)
            {
                response.StatusCode = HttpStatusCode.BadRequest;

                foreach (ValidationResult result in validationResults)
                    response.Messages.Add(result.ErrorMessage);
            }

            return response;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }
}
