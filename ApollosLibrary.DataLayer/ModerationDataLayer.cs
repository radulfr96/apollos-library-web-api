using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class ModerationDataLayer : IModerationDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public ModerationDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddEntryReport(EntryReport entryReport)
        {
            await _context.EntryReports.AddAsync(entryReport);
        }
    }
}
