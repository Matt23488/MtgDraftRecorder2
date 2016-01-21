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
    public partial class UserStore : IUserRoleStore<AspNetUser, int>
    {
        public Task AddToRoleAsync(AspNetUser user, string roleName) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var role = context.AspNetRoles.Single(obj => obj.Name == roleName);
                    var oldUser = context.AspNetUsers.Single(obj => obj.Id == user.Id);

                    oldUser.AspNetRoles.Add(role);
                    context.SaveChanges();
                }
                scope.Complete();
            }
        });

        public Task<IList<string>> GetRolesAsync(AspNetUser user) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return (IList<string>)
                    context.AspNetUsers
                    .SingleOrDefault(obj => obj.Id == user.Id)
                    ?.AspNetRoles
                    .Select(obj => obj.Name)
                    .ToList();
            }
        });

        public Task<bool> IsInRoleAsync(AspNetUser user, string roleName) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetUsers
                    .Single(obj => obj.Id == user.Id)
                    .AspNetRoles
                    .Select(obj => obj.Name)
                    .Contains(roleName);
            }
        });

        public Task RemoveFromRoleAsync(AspNetUser user, string roleName) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var oldUser = context.AspNetUsers.Single(obj => obj.Id == user.Id);
                    var role = oldUser.AspNetRoles.Single(obj => obj.Name == roleName);
                    oldUser.AspNetRoles.Remove(role);

                    context.SaveChanges();
                }
                scope.Complete();
            }
        });
    }
}
