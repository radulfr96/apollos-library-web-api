using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.DTOs
{
    public class PublisherDTO
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public CountryDTO Country { get; set; }
        public string Website { get; set; }
    }
}
