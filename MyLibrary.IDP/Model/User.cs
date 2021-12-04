using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.IDP.Model
{
    public partial class User
    {
        public User()
        {
            UserClaims = new HashSet<UserClaim>();
        }

        public Guid UserId { get; set; }
        public string Subject { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string SecurityCode { get; set; }
        public DateTime? SecurityCodeExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; set; }
    }
}
