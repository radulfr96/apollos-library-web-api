using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.IDP.Model
{
    public partial class ApiResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
