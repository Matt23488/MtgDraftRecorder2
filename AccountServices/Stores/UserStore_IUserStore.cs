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
    public partial class UserStore : IUserStore<AspNetUser, int>
    {
        public Task CreateAsync(AspNetUser user) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    context.AspNetUsers.Add(user);
                    context.SaveChanges();
                }
                scope.Complete();
            }
        });

        public Task DeleteAsync(AspNetUser user) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    context.AspNetUsers.Remove(user);
                    context.SaveChanges();
                }
                scope.Complete();
            }
        });

        public void Dispose() { }

        public Task<AspNetUser> FindByIdAsync(int userId) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUsers.SingleOrDefault(obj => obj.Id == userId);
            }
        });

        public Task<AspNetUser> FindByNameAsync(string userName) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUsers.SingleOrDefault(obj => obj.UserName == userName);
            }
        });

        public Task UpdateAsync(AspNetUser user) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var oldUser = context.AspNetUsers.SingleOrDefault(obj => obj.Id == user.Id);

                    if(oldUser != null)
                    {
                        context.Entry(oldUser).CurrentValues.SetValues(user);
                        context.SaveChanges();
                    }
                }
                scope.Complete();
            }
        });
    }
}
