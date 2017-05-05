namespace UniStore.App
{
    using System.Linq;
    using AutoMapper;
    using Models.BindingModels;
    using Models.BindingModels.Category;
    using Models.BindingModels.Departments;
    using Models.BindingModels.Manufacturer;
    using Models.BindingModels.Product;
    using Models.BindingModels.SubCategory;
    using Models.EntityModels;
    using Models.ViewModels.Admin;
    using Models.ViewModels.Category;
    using Models.ViewModels.Department;
    using Models.ViewModels.Invoice;
    using Models.ViewModels.Manufacturer;
    using Models.ViewModels.Product;
    using Models.ViewModels.Purchase;
    using Models.ViewModels.ShoppingCard;
    using Models.ViewModels.SubCategory;

    public class AutoMapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.Initialize(
                mapper =>
                {
                    mapper
                        .CreateMap<User, UserVM>()
                        .ForMember(
                            uvm => uvm.Roles,
                            opt => opt.MapFrom(
                                u => u.Roles.Select(r => r.RoleId)))
                        .Include<User, UserFullVM>();
                    mapper.CreateMap<User, UserFullVM>()
                        .ForMember(uvm => uvm.Address, opt => opt.MapFrom(u => u.Address))
                        .ForMember(uvm => uvm.PhoneNumber, opt => opt.MapFrom(u => u.PhoneNumber));
                    mapper.CreateMap<UserFullVM, EditUserVM>();
                    mapper.CreateMap<EditUserBM, EditUserVM>();

                    mapper.CreateMap<AddManufacturerBM, Manufacturer>();
                    mapper.CreateMap<Manufacturer, ManufacturerVM>();
                    mapper.CreateMap<EditManufacturerBM, ManufacturerVM>();

                    mapper.CreateMap<Category, CategoryVM>()
                        .ForMember(
                            vm => vm.ProductsCount,
                            opt => opt.MapFrom(
                                c => c.SubCategories.Sum(sc => sc.Products.Count)));
                    mapper.CreateMap<CategoryBM, Category>();
                    mapper.CreateMap<Category, RenameCategoryVM>();
                    mapper.CreateMap<Category, DetailsCategoryVM>();
                    mapper.CreateMap<Category, CategorySubCategoriesVM>()
                        .ForMember(
                            vm => vm.ProductsCount,
                            opt => opt.MapFrom(
                                c => c.SubCategories.Sum(sc => sc.Products.Count)));

                    mapper.CreateMap<Department, DepartmentVM>()
                        .ForMember(
                            vm => vm.ProductsCount,
                            opt => opt.MapFrom(
                                d => d.Categories
                                    .Sum(c => c.SubCategories.Sum(sc => sc.Products.Count))));
                    mapper.CreateMap<Department, DepartmentCategoriesVM>()
                        .ForMember(
                            vm => vm.ProductsCount,
                            opt => opt.MapFrom(
                                d => d.Categories
                                    .Sum(c => c.SubCategories.Sum(sc => sc.Products.Count))));
                    mapper.CreateMap<AddDepartmentBM, Department>();
                    mapper.CreateMap<Department, DetailsDepartmentVM>();
                    mapper.CreateMap<EditDepartmentBM, DepartmentVM>();

                    mapper.CreateMap<AddSubCategoryBM, AddSubCategoryVM>();
                    mapper.CreateMap<SubCategory, SubCategoryVM>();
                    mapper.CreateMap<SubCategory, RenameSubCategoryVM>()
                        .ForMember(
                            vm => vm.DepartmentId,
                            opt => opt.MapFrom(sc => sc.Category.Department.Id));
                    mapper.CreateMap<RenameSubCategoryBM, RenameSubCategoryVM>();
                    mapper.CreateMap<SubCategory, DetailsSubCategoryVM>()
                        .ForMember(
                            vm => vm.DepartmentId,
                            opt => opt.MapFrom(sc => sc.Category.Department.Id));
                    mapper.CreateMap<SubCategory, SubCategoryProductsVM>()
                        .ForMember(
                            vm => vm.DepartmentId,
                            opt => opt.MapFrom(sc => sc.Category.Department.Id))
                        .ForMember(
                            vm => vm.DepartmentName,
                            opt => opt.MapFrom(sc => sc.Category.Department.Name));
                    mapper.CreateMap<Product, ProductVM>();
                    mapper.CreateMap<Product, DetailsProductVM>();
                    mapper.CreateMap<AddProductBM, AddProductVM>();
                    mapper.CreateMap<AddProductBM, Product>()
                        .ConstructUsing(x => new Product());

                    // todo: construct is needed?
                    mapper.CreateMap<Product, EditProductVM>();
                    mapper.CreateMap<EditProductBM, EditProductVM>();

                    mapper.CreateMap<Purchase, PurchaseVM>();

                    mapper.CreateMap<Invoice, InvoiceVM>();

                    mapper.CreateMap<ShoppingCart, ShoppingCardVM>();
                }
            );
        }
    }
}