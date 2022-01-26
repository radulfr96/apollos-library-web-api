using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using ApollosLibrary.IDP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.Stores
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMapper _mapper;

        public DeviceFlowStore(ApollosLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IdentityServer4.Models.DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var code = await _context.DeviceCodes.FirstOrDefaultAsync(d => d.DeviceCode1 == deviceCode);

            return _mapper.Map<Model.DeviceCode, IdentityServer4.Models.DeviceCode>(code);
        }

        public async Task<IdentityServer4.Models.DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var code = await _context.DeviceCodes.FirstOrDefaultAsync(d => d.UserCode == userCode);

            return _mapper.Map<Model.DeviceCode, IdentityServer4.Models.DeviceCode>(code);
        }

        public async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var code = await _context.DeviceCodes.FirstOrDefaultAsync(d => d.UserCode == deviceCode);

            if (code != null)
            {
                _context.Remove(code);
                await _context.SaveChangesAsync();
            }
        }

        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, IdentityServer4.Models.DeviceCode data)
        {

            var entity = new Model.DeviceCode()
            {
                ClientId = data.ClientId,
                CreationTime = data.CreationTime,
                Data = JsonConvert.SerializeObject(data),
                Description = data.Description,
                DeviceCode1 = deviceCode,
                Expiration = data.CreationTime.AddYears(1),
                SessionId = data.SessionId,
                UserCode = userCode,
                SubjectId = data.Subject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value,
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByUserCodeAsync(string userCode, IdentityServer4.Models.DeviceCode data)
        {
            var entity = await _context.DeviceCodes.FirstOrDefaultAsync(c => c.UserCode == userCode);
            entity.ClientId = data.ClientId;
            entity.CreationTime = data.CreationTime;
            entity.Data = JsonConvert.SerializeObject(data);
            entity.Description = data.Description;
            entity.Expiration = data.CreationTime.AddYears(1);
            entity.SessionId = data.SessionId;
            entity.UserCode = userCode;
            entity.SubjectId = data.Subject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;

            await _context.SaveChangesAsync();
        }
    }
}
