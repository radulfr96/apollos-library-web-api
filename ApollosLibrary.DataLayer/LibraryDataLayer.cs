using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class LibraryDataLayer : ILibraryDataLayer
    {
        public ApollosLibraryContext _context;

        public LibraryDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddLibrary(Library library)
        {
            await _context.Libraries.AddAsync(library);
        }

        public async Task AddLibraryEntry(LibraryEntry entry)
        {
            await _context.LibraryEntries.AddAsync(entry);
        }

        public async Task DeleteLibraryEntry(int id)
        {
            var library = await _context.Libraries.FirstOrDefaultAsync(l => l.LibraryId == id);

            if (library != null)
            {
                await Task.Run(() =>
                {
                    _context.Libraries.Remove(library);
                });
            }
        }

        public async Task<Library> GetLibrary(int libraryId)
        {
            return await _context.Libraries.FirstOrDefaultAsync(l => l.LibraryId == libraryId);
        }

        public async Task<List<LibraryEntry>> GetLibraryEntriesByUserId(Guid userId)
        {
            return await _context.LibraryEntries
                                    .Include(l => l.Library)
                                    .Include(l => l.Book)
                                    .ThenInclude(b => b.Authors)
                                    .Where(l => l.Library.UserId == userId).ToListAsync();
        }

        public async Task<LibraryEntry> GetLibraryEntry(int libraryId, int bookId)
        {
            return await _context.LibraryEntries.FirstOrDefaultAsync(e => e.LibraryId == libraryId && e.BookId == bookId);
        }

        public async Task<LibraryEntry> GetLibraryEntry(int id)
        {
            return await _context.LibraryEntries
                                    .Include(e => e.Library)
                                    .FirstOrDefaultAsync(l => l.EntryId == id);
        }

        public async Task<int?> GetLibraryIdByUserId(Guid userId)
        {
            return (await _context.Libraries.FirstOrDefaultAsync(l => l.UserId == userId))?.LibraryId;
        }
    }
}
