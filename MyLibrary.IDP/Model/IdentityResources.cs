using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MyLibrary.IDP.Model
{
    [Table("IdentityResources", Schema = "Identity")]
    public partial class IdentityResources
    {
        public IdentityResources()
        {
            IdentityResourceClaims = new HashSet<IdentityResourceClaim>();
            IdentityResourceProperties = new HashSet<IdentityResourceProperty>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool NonEditable { get; set; }

        public virtual ICollection<IdentityResourceClaim> IdentityResourceClaims { get; set; }
        public virtual ICollection<IdentityResourceProperty> IdentityResourceProperties { get; set; }
    }
}
