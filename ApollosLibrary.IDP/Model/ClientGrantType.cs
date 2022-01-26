using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.IDP.Model
{
    public partial class ClientGrantType
    {
        public int Id { get; set; }
        public string GrantType { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
