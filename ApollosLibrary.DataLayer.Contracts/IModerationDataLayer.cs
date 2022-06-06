using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    public interface IModerationDataLayer
    {
        Task AddEntryReport(EntryReport entryReport);
    }
}
