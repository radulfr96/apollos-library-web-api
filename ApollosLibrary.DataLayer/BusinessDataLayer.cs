﻿using Microsoft.EntityFrameworkCore;
using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApollosLibrary.Domain;

namespace ApollosLibrary.DataLayer
{
    public class BusinessDataLayer : IBusinessDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public BusinessDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddBusiness(Business Business)
        {
            await _context.Business.AddAsync(Business);
        }

        public async Task AddBusinessRecord(BusinessRecord record)
        {
            await _context.BusinessRecords.AddAsync(record);
        }

        public async Task<Business> GetBusiness(int id)
        {
            return await _context.Business.FirstOrDefaultAsync(b => b.BusinessId == id && !b.IsDeleted);
        }

        public async Task<BusinessRecord> GetBusinessRecord(int recordId)
        {
            return await _context.BusinessRecords
                .Where(b => b.BusinessRecordId == recordId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Business>> GetBusinesses()
        {
            return await _context.Business.Include(b => b.Country).Include(b => b.Type).Where(b => !b.IsDeleted).ToListAsync();
        }
    }
}
