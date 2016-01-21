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
    public class RoleStore : IRoleStore<AspNetRole, int>
    {
        public Task CreateAsync(AspNetRole role) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    context.AspNetRoles.Add(role);
                    context.SaveChanges();
                }
                scope.Complete();
            }
        });

        public Task DeleteAsync(AspNetRole role) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var oldRole = context.AspNetRoles.Single(obj => obj.Id == role.Id);
                    context.AspNetRoles.Remove(oldRole);

                    context.SaveChanges();
                }
                scope.Complete();
            }
        });

        public void Dispose() { }

        public Task<AspNetRole> FindByIdAsync(int roleId) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetRoles.SingleOrDefault(obj => obj.Id == roleId);
            }
        });

        public Task<AspNetRole> FindByNameAsync(string roleName) => Task.Factory.StartNew(() =>
        {
            using (var context = new AccountServicesModelContainer())
            {
                return context.AspNetRoles.SingleOrDefault(obj => obj.Name == roleName);
            }
        });

        public Task UpdateAsync(AspNetRole role) => Task.Factory.StartNew(() =>
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new AccountServicesModelContainer())
                {
                    var oldRole = context.AspNetRoles.Single(obj => obj.Id == role.Id);
                    context.Entry(oldRole).CurrentValues.SetValues(role);

                    context.SaveChanges();
                }
                scope.Complete();
            }
        });
    }
}
