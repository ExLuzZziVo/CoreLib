#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.ASP.Resources;
using CoreLib.CORE.CustomObjects;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

#endregion

namespace CoreLib.ASP.Helpers.IdentityHelpers
{
    public class RoleManagerHelper<T> where T : IdentityRole<string>, new()
    {
        private readonly IMemoryCache _memoryCache;
        private readonly RoleManager<T> _roleManager;

        public RoleManagerHelper(RoleManager<T> roleManager, IMemoryCache memoryCache)
        {
            _roleManager = roleManager;
            _memoryCache = memoryCache;
        }

        public async Task AddIdentityRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new T {Name = roleName});
            if (result.Succeeded)
                RefreshIdentityRoleNameCache();
            else
                throw new ExtendedValidationException(result.Errors.Select(e => e.Description));
        }

        public async Task DeleteIdentityRoleAsync(string id)
        {
            if (id.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), IdentityResources.ResourceManager.GetString("IdentityRoleNoIdError"));
            var roleToDelete = await _roleManager.Roles.SingleOrDefaultAsync(r =>
                r.Id == id && !(r.Name == "Administrators" || r.Name == "SimpleUsers"));
            if (roleToDelete == null)
                throw new NullReferenceException(IdentityResources.ResourceManager.GetString("IdentityRoleNotFoundError"));
            var result = await _roleManager.DeleteAsync(roleToDelete);
            if (result.Succeeded)
                RefreshIdentityRoleNameCache();
        }

        public bool VerifyIdentityRoleName(string name)
        {
            if (!_memoryCache.TryGetValue(CacheKeys.IdentityRoleNames,
                out IDictionary<string, string> identityRoleNames))
                identityRoleNames = RefreshIdentityRoleNameCache();

            return identityRoleNames.Values.Any(n => n == name);
        }

        private IDictionary<string, string> RefreshIdentityRoleNameCache()
        {
            var roleNames = _roleManager.Roles.ToDictionary(s => s.Id, s => s.Name);
            if (roleNames.Count() != 0) _memoryCache.Set(CacheKeys.IdentityRoleNames, roleNames);
            return roleNames;
        }
    }
}