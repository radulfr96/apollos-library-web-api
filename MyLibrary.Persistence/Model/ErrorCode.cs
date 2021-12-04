using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.Persistence.Model
{
    public partial class ErrorCode
    {
        public int ErrorCodeId { get; set; }
        public string Message { get; set; }
    }
}
