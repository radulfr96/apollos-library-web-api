using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.IDP.Model
{
    public partial class IdentityResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int IdentityResourceId { get; set; }

        public virtual IdentityResources IdentityResource { get; set; }
    }
}
