using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace MyLibrary.Common.Requests
{
    /// <summary>
    /// Used as the base for all requests includes base validation functionality for all requests
    /// </summary>
    public class BaseRequest : IValidatableObject
    {
        /// <summary>
        /// Used to validate the request and return the results in the response received
        /// </summary>
        /// <param name="response">The response with errors if there are any</param>
        /// <returns>The base response object</returns>
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

        /// <summary>
        /// Used to implement validation not able to be validated in data annotations
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns>The list of validation errors if there are any</returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }
}
