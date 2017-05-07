namespace UniStore.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.BindingModels.User;
    using Models.EntityModels;
    using Models.ViewModels.User;

    public class AdminService : BaseService, IAdminService
    {
        public AdminService(IUniStoreContext context) : base(context)
        {
        }

        public UserVM GetUserVM(string username = null, User user = null)
        {
            if (user == null)
            {
                user = this.Context.Users.All().FirstOrDefault(u => u.UserName.Equals(username));
            }

            if (user == null)
            {
                return null;
            }

            var userVM = Mapper.Map<UserVM>(user);
            userVM.Roles = this.GetRoleNames(userVM.Roles).ToList();

            return userVM;
        }

        public IEnumerable<UserVM> GetAllUserVMs()
        {
            var users = this.Context.Users.All().ToList();
            var userVMs = Mapper.Map<IList<UserVM>>(users);

            foreach (var userVM in userVMs)
            {
                userVM.Roles = this.GetRoleNames(userVM.Roles).ToList();
            }

            return userVMs;
        }

        public UserFullVM GetUserFullVM(string username)
        {
            var user = this.Context.Users.All().FirstOrDefault(u => u.UserName.Equals(username));
            if (user == null)
            {
                return null;
            }

            var userVM = Mapper.Map<UserFullVM>(user);
            userVM.Roles = this.GetRoleNames(userVM.Roles).ToList();

            return userVM;
        }

        public EditUserVM GetEditUserVM(string username)
        {
            var userVM = Mapper.Map<EditUserVM>(this.GetUserFullVM(username));
            if (userVM == null)
            {
                return null;
            }

            var appRoles = this.Context.Roles.All().Select(r => r.Name).ToList();
            userVM.AppRoles = new SelectList(appRoles);

            return userVM;
        }


        public void UpdateUserData(EditUserBM userBM, bool isAdmin)
        {
            var user = this.Context.Users.Find(userBM.Id);
            if (user == null)
            {
                throw new ArgumentException("Not found edited user in database!");
            }

            user.Address = userBM.Address;
            user.PhoneNumber = userBM.PhoneNumber;
            user.Name = userBM.Name;
            if (isAdmin)
            {
                var oldRoles = this.GetRoleNames(user.Roles.Select(ur => ur.RoleId).ToList());

                foreach (var role in oldRoles)
                {
                    if (!userBM.Roles.Contains(role))
                    {
                        this.Context.UserManager.RemoveFromRole(user.Id, role);
                    }
                }


                if (!string.IsNullOrEmpty(userBM.NewRole)
                    && this.Context.Roles.All().Any(r => r.Name.Equals(userBM.NewRole))
                    && !userBM.Roles.Contains(userBM.NewRole))
                {
                    this.Context.UserManager.AddToRole(userBM.Id, userBM.NewRole);
                }
            }

            this.Context.SaveChanges();
        }

        private IEnumerable<string> GetRoleNames(IEnumerable<string> roleIds)
        {
            var appRoles = this.Context.Roles.All().ToList();
            var roles = roleIds.Select(r => appRoles.First(ar => ar.Id == r).Name);
            return roles;
        }

        public void RemoveUser(string username, string cuurentUserName)
        {
            var user = this.Context.Users.All().FirstOrDefault(u => u.UserName.Equals(username));
            if (user == null || user.UserName.Equals(cuurentUserName))
            {
                return;
            }

            this.Context.UserManager.Delete(user);
        }
    }
}