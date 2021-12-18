using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.Persistence.Model
{
    public partial class UserClaim
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
