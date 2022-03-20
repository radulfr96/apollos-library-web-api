using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.DTOs
{
    public class BookReferenceDataDTO
    {
        public List<FormType> FormTypes { get; set; } = new List<FormType>();
        public List<PublicationFormat> PublicationFormats { get; set; } = new List<PublicationFormat>();
        public List<FictionType> FictionTypes { get; set; } = new List<FictionType>();
    }
}
