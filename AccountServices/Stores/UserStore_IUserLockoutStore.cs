using AccountServices.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServices.Stores
{
    public partial class UserStore : IUserLockoutStore<AspNetUser, int>
    {
        public Task<int> GetAccessFailedCountAsync(AspNetUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(AspNetUser user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AspNetUser user)
        {
            return Task.FromResult(DateTimeOffset.Now);
        }

        public Task<int> IncrementAccessFailedCountAsync(AspNetUser user)
        {
            return Task.FromResult(1);
        }

        public Task ResetAccessFailedCountAsync(AspNetUser user)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetLockoutEnabledAsync(AspNetUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetLockoutEndDateAsync(AspNetUser user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult<object>(null);
        }
    }
}
