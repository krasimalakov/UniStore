namespace UniStore.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.EntityModels;
    using Models.Enums;

    public sealed class Configuration : DbMigrationsConfiguration<UniStoreEfDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "UniStore.Data.UniStoreDbContext";
        }

        protected override void Seed(UniStoreEfDbContext context)
        {
            SeedRolesAndAdmin(context);
            SeedManufacturers(context);
            SeedData(context);
        }

        private static void SeedManufacturers(UniStoreEfDbContext context)
        {
            var manufacturers = new Dictionary<string, string>
            {
                { "IBM", "гр.София, бул.Витоша 33, office@ibm.com" },
                { "Dell", "0888888888, office@dell.com" },
                { "HP Co", "025551144, salles@hp.com" },
                { "AMD", "office@amd.com" },
                { "Правец", "гр.Правец, ул.Люляк 15, 0888555555, office@pravetz.bg" },
                { "Canon", "02/555-1234, contacts@canon.bg" },
            };
            if (context.Manufacturers.Any())
            {
                return;
            }

            foreach (var manufacturerData in manufacturers)
            {
                var manufacturer = new Manufacturer
                {
                    Name = manufacturerData.Key,
                    ContactInformation = manufacturerData.Value
                };
                context.Manufacturers.Add(manufacturer);
                context.SaveChanges();
            }
        }

        private static void SeedData(UniStoreEfDbContext context)
        {
            var products = new List<SeedProduct>
            {
                NewProduct("Computers", "Desktop", "Home", "Computer-1", 100.22m, 3, 1),
                NewProduct("Computers", "Desktop", "Home", "Computer-2", 150.45m, 5, 2),

                NewProduct("Computers", "Desktop", "Business", "Computer-3", 200, 13, 1),
                NewProduct("Computers", "Desktop", "Business", "Computer-4", 125.05m, 32, 2),
                NewProduct("Computers", "Desktop", "Business", "Computer-5", 225, 5, 3),
                NewProduct("Computers", "Desktop", "Business", "Computer-6", 925, 1, 5),

                NewProduct("Computers", "Desktop", "Gamers", "Computer-7", 600, 5, 2),
                NewProduct("Computers", "Desktop", "Gamers", "Computer-8", 700, 15, 3),
                NewProduct("Computers", "Desktop", "Gamers", "Computer-9", 800, 7, 4),
                NewProduct("Computers", "Desktop", "Gamers", "Computer-10", 1200, 2, 5),

                NewProduct("Computers", "Laptops", "Gamers", "Laptop-1", 1200, 12, 1),
                NewProduct("Computers", "Laptops", "Gamers", "Laptop-2", 2005, 45, 2),
                NewProduct("Computers", "Laptops", "Gamers", "Laptop-3", 3200, 1, 5),

                NewProduct("Computers", "Laptops", "Business", "Laptop-4", 1300, 1, 1),
                NewProduct("Computers", "Laptops", "Business", "Laptop-5", 1450, 11, 1),
                NewProduct("Computers", "Laptops", "Business", "Laptop-6", 3300, 21, 2),

                NewProduct("Peripheral devices", "Printers", "Laser", "Printer-1", 300, 20, 1),
                NewProduct("Peripheral devices", "Printers", "Laser", "Printer-2", 1000, 5, 6),
                NewProduct("Peripheral devices", "Printers", "Inkjet", "Printer-3", 100, 15, 1)
            };
            if (!context.Departments.Any())
            {
                var manufacturers = context.Manufacturers.ToList();
                foreach (var seedProduct in products)
                {
                    var department = context.Departments
                        .FirstOrDefault(d => d.Name.Equals(seedProduct.Department));
                    if (department == null)
                    {
                        department = new Department { Name = seedProduct.Department };
                        context.Departments.Add(department);
                        context.SaveChanges();
                    }

                    var category = department.Categories
                        .FirstOrDefault(c => c.Name.Equals(seedProduct.Category));
                    if (category == null)
                    {
                        category = new Category { Name = seedProduct.Category };
                        department.Categories.Add(category);
                        context.SaveChanges();
                    }

                    var subCategory = category.SubCategories
                        .FirstOrDefault(sc => sc.Name.Equals(seedProduct.SubCategory));
                    if (subCategory == null)
                    {
                        subCategory = new SubCategory { Name = seedProduct.SubCategory };
                        category.SubCategories.Add(subCategory);
                        context.SaveChanges();
                    }

                    var product = subCategory.Products
                        .FirstOrDefault(p => p.Name.Equals(seedProduct.Name));
                    if (product == null)
                    {
                        product = new Product
                        {
                            Name = seedProduct.Name,
                            Price = seedProduct.Price,
                            Quantity = seedProduct.Quantity,
                            Description = "Brand, model, parameters, product description ...",
                            Manufacturer = manufacturers.First(m => m.Id == seedProduct.ManufacturerId)
                        };
                        subCategory.Products.Add(product);
                        context.SaveChanges();
                    }
                }
            }
        }

        private static SeedProduct NewProduct(
            string department,
            string category,
            string subCategory,
            string name,
            decimal price,
            int quantity,
            int manufacturerId)
        {
            return new SeedProduct
            {
                Department = department,
                Category = category,
                SubCategory = subCategory,
                Name = name,
                Price = price,
                Quantity = quantity,
                ManufacturerId = manufacturerId
            };
        }

        private static void SeedRolesAndAdmin(UniStoreEfDbContext context)
        {
            var appRoles = Enum.GetNames(typeof(AppRole));
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            foreach (var appRole in appRoles)
            {
                if (!context.Roles.Any(r => r.Name == appRole))
                {
                    var role = new IdentityRole { Name = appRole };
                    roleManager.Create(role);
                }
            }

            var adminRoleName = appRoles[0];
            var adminRoleId = context.Roles.First(r => string.Equals(r.Name, adminRoleName)).Id;

            if (!context.Users.Any(u => u.Roles.Any(r => r.RoleId == adminRoleId)))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var user = new User { UserName = "admin", Email = "admin@admin.app" };

                userManager.Create(user, "123456");
                var userId = user.Id;
                userManager.AddToRole(userId, AppRole.Administrator.ToString());
            }
        }
    }
    internal class SeedProduct
    {
        public string Department { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ManufacturerId { get; set; }
    }
}