using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using MyLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.IDP.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly MyLibraryContext _context;
        private readonly IMapper _mapper;

        public ClientStore(MyLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            var clientEntity = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);

            var client = _mapper.Map<IdentityServer4.Models.Client>(clientEntity);

            return client;
        }
    }
}
