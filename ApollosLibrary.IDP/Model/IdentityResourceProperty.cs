using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.IDP.Model
{
    public partial class IdentityResourceProperty
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int IdentityResourceId { get; set; }

        public virtual IdentityResource IdentityResource { get; set; }
    }
}
