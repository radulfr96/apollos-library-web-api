using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class UpdatePublisherRequest : BaseRequest
    {
        public int PublisherID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a publisher name")]
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a country")]
        public string CountryID { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Website))
            {
                if (string.IsNullOrEmpty(StreetAddress)
                    && string.IsNullOrEmpty(City)
                    && string.IsNullOrEmpty(Postcode)
                    && string.IsNullOrEmpty(State))
                {
                    results.Add(new ValidationResult("You must provide the publisher address or website."));
                }
            }

            if ((!string.IsNullOrEmpty(StreetAddress)
                    || !string.IsNullOrEmpty(City)
                    || !string.IsNullOrEmpty(Postcode)
                    || !string.IsNullOrEmpty(State)
                    )
                    &&
                    (
                    string.IsNullOrEmpty(StreetAddress)
                    || string.IsNullOrEmpty(City)
                    || string.IsNullOrEmpty(Postcode))
                    )
            {
                results.Add(new ValidationResult("You must provide a full address."));
            }

            return results;
        }
    }
}
