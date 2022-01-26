using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using ApollosLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMapper _mapper;

        public ClientStore(ApollosLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            var clientEntity = await _context.Clients
                                             .Include("ClientGrantTypes")
                                             .Include("ClientRedirectUris")
                                             .Include("ClientSecrets")
                                             .Include("ClientScopes")
                                             .FirstOrDefaultAsync(c => c.ClientId == clientId);

            var client = _mapper.Map<IdentityServer4.Models.Client>(clientEntity);
            client.AllowedGrantTypes = clientEntity.ClientGrantTypes.Select(c => c.GrantType).ToList();
            client.RedirectUris = clientEntity.ClientRedirectUris.Select(c => c.RedirectUri).ToList();
            client.ClientSecrets = clientEntity.ClientSecrets.Select(c => new Secret()
            {
                Description = c.Description,
                Value = c.Value,
                Type = c.Type,
            }).ToList();
            client.AllowedScopes = clientEntity.ClientScopes.Select(s => s.Scope).ToList();


            return client;
        }
    }
}
