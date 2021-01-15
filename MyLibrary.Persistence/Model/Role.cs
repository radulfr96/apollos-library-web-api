using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
