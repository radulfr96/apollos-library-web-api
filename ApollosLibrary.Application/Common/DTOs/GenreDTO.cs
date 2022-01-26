using System;
using System.Collections.Generic;
using System.Text;

namespace ApollosLibrary.Application.Common.DTOs
{
    /// <summary>
    /// Used to transfer genre data in the system
    /// </summary>
    public class GenreDTO
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}
