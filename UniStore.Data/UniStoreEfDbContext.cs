namespace UniStore.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.EntityModels;

    public class UniStoreEfDbContext : IdentityDbContext<User>
    {
        public UniStoreEfDbContext()
            : base("UniStoreEfDb", false)
        {
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Order> Orders { get; set; }

        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOptional(x => x.Department)
                .WithMany(x => x.Categories)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SubCategory>()
                .HasOptional(x => x.Category)
                .WithMany(x => x.SubCategories)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Product>()
                .HasOptional(x => x.SubCategory)
                .WithMany(x => x.Products)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        public static UniStoreEfDbContext Create()
        {
            return new UniStoreEfDbContext();
        }
    }
}