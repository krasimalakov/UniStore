namespace UniStore.Data.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.EntityModels;
    using Repositories;

    public class UniStoreContext : IUniStoreContext
    {
        private readonly DbContext dbContext;
        private readonly IDictionary<Type, object> repositories;
        private RoleStore<IdentityRole> roleStore;
        private UserManager<User> userManager;
        private IUserStore<User> userStore;

        public UniStoreContext(DbContext context)
        {
            this.dbContext = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users =>
            this.GetRepository<User>();

        public IRepository<IdentityRole> Roles =>
            this.GetRepository<IdentityRole>();

        public IRepository<Manufacturer> Manufacturers => 
            this.GetRepository<Manufacturer>();

        public IRepository<Department> Departments =>
            this.GetRepository<Department>();

        public IRepository<Category> Categories => 
            this.GetRepository<Category>();

        public IRepository<SubCategory> SubCategories =>
            this.GetRepository<SubCategory>();

        public IRepository<Product> Products => 
            this.GetRepository<Product>();

        public IRepository<Purchase> Purchases =>
            this.GetRepository<Purchase>();

        public IRepository<Order> Orders =>
            this.GetRepository<Order>();

        public IRepository<ShoppingCart> ShoppingCarts =>
            this.GetRepository<ShoppingCart>();

        public IUserStore<User> UserStore =>
            this.userStore ?? (this.userStore = new UserStore<User>(this.dbContext));

        public UserManager<User> UserManager =>
            this.userManager ?? (this.userManager = new UserManager<User>(this.UserStore));

        public RoleStore<IdentityRole> RoleStore =>
            this.roleStore ?? (this.roleStore = new RoleStore<IdentityRole>(this.dbContext));

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericEfRepository<T>);
                this.repositories.Add(
                    typeof(T),
                    Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}