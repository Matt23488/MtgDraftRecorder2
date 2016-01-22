using AccountServices.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServices.Stores
{
    public partial class UserStore : IUserTwoFactorStore<AspNetUser, int>
    {
        public Task<bool> GetTwoFactorEnabledAsync(AspNetUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(AspNetUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }
    }
}
