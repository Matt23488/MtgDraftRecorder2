using AccountServices.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AccountServices.Stores
{
    public partial class UserStore : IUserLoginStore<AspNetUser, int>
    {
        public Task AddLoginAsync(AspNetUser user, UserLoginInfo login) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    context.AspNetUserLogins.Add(new AspNetUserLogin
                    {
                        UserId = user.Id,
                        LoginProvider = login.LoginProvider,
                        ProviderKey = login.ProviderKey
                    });
                }
                scope.Complete();
            }
        });

        public Task<AspNetUser> FindAsync(UserLoginInfo login) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUserLogins
                    .SingleOrDefault(obj => obj.LoginProvider == login.LoginProvider && obj.ProviderKey == login.ProviderKey)
                    ?.AspNetUser;
            }
        });

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AspNetUser user) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return (IList<UserLoginInfo>)
                    context.AspNetUsers
                    .Single(obj => obj.Id == user.Id)
                    .AspNetUserLogins
                    .Select(obj => new UserLoginInfo(obj.LoginProvider, obj.ProviderKey))
                    .ToList();
            }
        });

        public Task RemoveLoginAsync(AspNetUser user, UserLoginInfo login) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var oldUser = context.AspNetUsers.Single(obj => obj.Id == user.Id);
                    var oldLogin = oldUser.AspNetUserLogins.Single(obj => obj.LoginProvider == login.LoginProvider && obj.ProviderKey == login.ProviderKey);

                    oldUser.AspNetUserLogins.Remove(oldLogin);
                    context.SaveChanges();
                }
                scope.Complete();
            }
        });
    }
}
