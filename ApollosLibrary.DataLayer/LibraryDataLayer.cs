using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class LibraryDataLayer : ILibraryDataLayer
    {
        public Task AddLibrary(Library library)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLibraryEntry(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LibraryEntry>> GetLibrary(int libraryId)
        {
            throw new NotImplementedException();
        }

        public Task<LibraryEntry> GetLibraryEntry(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLibraryIdByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
