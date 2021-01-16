using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Commands.AddBookCommand
{
    public class AddBookCommand : IRequest<AddBookCommandDto>
    {
        public string ISBN { get; set; }
        public string eISBN { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a title")]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesID { get; set; }
        public int? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatID { get; set; }
        public int FictionTypeID { get; set; }
        public int FormTypeID { get; set; }
        public int PublisherIDs { get; set; }
        public byte[] CoverImage { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
        public List<int> Authors { get; set; } = new List<int>();

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(eISBN) && string.IsNullOrEmpty(ISBN))
            {
                results.Add(new ValidationResult("You must provide a ISBN or an eISBN"));
            }

            return results;
        }
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, AddBookCommandDto>
    {
        public Task<AddBookCommandDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
