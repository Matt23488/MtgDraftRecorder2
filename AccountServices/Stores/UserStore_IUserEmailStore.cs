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
    public partial class UserStore : IUserEmailStore<AspNetUser, int>
    {
        public Task<AspNetUser> FindByEmailAsync(string email) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUsers.SingleOrDefault(obj => obj.Email == email);
            }
        });

        public Task<string> GetEmailAsync(AspNetUser user) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                var oldUser = context.AspNetUsers.SingleOrDefault(obj => obj.Id == user.Id);

                return oldUser == null ? user.Email : oldUser.Email;
            }
        });

        public Task<bool> GetEmailConfirmedAsync(AspNetUser user) => Task.FromResult(true);

        public Task SetEmailAsync(AspNetUser user, string email) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var oldUser = context.AspNetUsers.Single(obj => obj.Id == user.Id);

                    oldUser.Email = email;
                    context.SaveChanges();
                }
                scope.Complete();
            }
        });

        public Task SetEmailConfirmedAsync(AspNetUser user, bool confirmed) => Task.FromResult<object>(null);

        Task<AspNetUser> IUserStore<AspNetUser, int>.FindByIdAsync(int userId) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUsers.SingleOrDefault(obj => obj.Id == userId);
            }
        });

        Task<AspNetUser> IUserStore<AspNetUser, int>.FindByNameAsync(string userName) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUsers.SingleOrDefault(obj => obj.UserName == userName);
            }
        });
    }
}
