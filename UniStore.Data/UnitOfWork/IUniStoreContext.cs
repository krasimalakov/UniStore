namespace UniStore.Data.UnitOfWork
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.EntityModels;
    using Repositories;

    public interface IUniStoreContext
    {
        IUserStore<User> UserStore { get; }

        UserManager<User> UserManager { get; }

        RoleStore<IdentityRole> RoleStore { get; }

        IRepository<User> Users { get; }

        IRepository<IdentityRole> Roles { get; }

        IRepository<Manufacturer> Manufacturers { get; }

        IRepository<Department> Departments { get; }

        IRepository<Category> Categories { get; }

        IRepository<SubCategory> SubCategories { get; }

        IRepository<Product> Products { get; }

        IRepository<Purchase> Purchases { get; }

        IRepository<Invoice> Invoices { get; }

        IRepository<ShoppingCart> ShoppingCarts { get; }

        void SaveChanges();
    }
}