using AutoMapper;
using ApollosLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP
{
    public static class AutoMapper
    {
        public static MapperConfiguration Configuration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, IdentityServer4.Models.Client>();
                cfg.CreateMap<IdentityResource, IdentityServer4.Models.IdentityResource>();
                cfg.CreateMap<ApiResource, IdentityServer4.Models.ApiResource>();
                cfg.CreateMap<ApiScope, IdentityServer4.Models.ApiScope>();
                cfg.CreateMap<ApiResource, IdentityServer4.Models.Resource>();
                cfg.CreateMap<ApiScope, IdentityServer4.Models.ApiScope>();
                cfg.CreateMap<PersistedGrant, IdentityServer4.Models.PersistedGrant>();
                cfg.CreateMap<ClientSecret, IdentityServer4.Models.Secret>();
                cfg.CreateMap<ClientScope, IdentityServer4.Models.ApiScope>();
            });
        }
    }
}
