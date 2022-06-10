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

        public async Task<List<EntryReport>> GetEntryReports()
        {
            return await _context.EntryReports.ToListAsync();
        }

        public async Task<EntryReport> GetEntryReport(int entryReportId)
        {
            return await _context.EntryReports.FirstOrDefaultAsync(r => r.EntryReportId == entryReportId);
        }

        public async Task<List<EntryReport>> GetUsersEntryReports(Guid userId)
        {
            return await _context.EntryReports.Where(r => r.ReportedBy == userId).ToListAsync();
        }

        public async Task<List<EntryReport>> GetReportsOfEntriesByUser(Guid userId)
        {
            return await _context.EntryReports.Where(r => r.CreatedBy == userId).ToListAsync();
        }
    }
}
