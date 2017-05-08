namespace UniStore.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Models;
    using Models.BindingModels.Product;
    using Models.EntityModels;
    using Models.ViewModels.Product;
    using Models.ViewModels.Purchase;
    using Models.ViewModels.ShoppingCard;

    public class StoreService : BaseService, IStoreService
    {
        public StoreService(IUniStoreContext context) : base(context)
        {
        }

        public ProductsListVM GetProductsListVM(SearchProductsBM searchBM, string userId)
        {
            const int PageSize = 4;


            var search = new SearchProductsBM
            {
                Search = searchBM.Search ?? string.Empty,
                Page = searchBM.Page,
                Order = searchBM.Order ?? Constants.Order[0],
                OrderBy = searchBM.OrderBy ?? Constants.OrderBy[0],
                SubCategoryId = searchBM.SubCategoryId,
                CategoryId = searchBM.CategoryId,
                DepartmentId = searchBM.DepartmentId,
                OrderList = new SelectList(Constants.Order),
                OrderByList = new SelectList(Constants.OrderBy)
            };

            var pageNumber = search.Page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            IEnumerable<Product> products;
            search.SubCategoryId = searchBM.SubCategoryId;

            if (search.DepartmentId != 0)
            {
                products = this.Context.Products.All()
                    .Where(p => p.SubCategory.Category.Department.Id == search.DepartmentId);
                search.SubCategoryId = 0;
                search.CategoryId = 0;
            }
            else if (search.CategoryId != 0)
            {
                products = this.Context.Products.All()
                    .Where(p => p.SubCategory.Category.Id == search.CategoryId);
                search.SubCategoryId = 0;
                search.DepartmentId = 0;
            }
            else if (search.SubCategoryId != 0)
            {
                var subCategory = this.Context.SubCategories.Find(search.SubCategoryId);
                if (subCategory == null)
                {
                    return null;
                }

                products = subCategory.Products;
                search.DepartmentId = 0;
                search.CategoryId = 0;
            }
            else
            {
                products = this.Context.Products.All();
            }

            if (!string.IsNullOrEmpty(search.Search))
            {
                products = products
                    .Where(p => p.Name.ToLower().Contains(search.Search.ToLower()));
            }

            if (search.OrderBy == Constants.OrderBy[0])
            {
                products = string.Equals(search.Order, Constants.Order[0])
                    ? products.OrderBy(p => p.Id)
                    : products.OrderByDescending(p => p.Id);
            }
            else if (search.OrderBy == Constants.OrderBy[1])
            {
                products = string.Equals(search.Order, Constants.Order[0])
                    ? products.OrderBy(p => p.Name)
                    : products.OrderByDescending(p => p.Name);
            }
            else if (search.OrderBy == Constants.OrderBy[2])
            {
                products = string.Equals(search.Order, Constants.Order[0])
                    ? products.OrderBy(p => p.Price)
                    : products.OrderByDescending(p => p.Price);
            }

            var productVms = products
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(Mapper.Map<DetailsProductVM>);

            var productsCount = products.Count();
            var pageCount = productsCount / PageSize + (productsCount % PageSize > 0 ? 1 : 0);

            search.PageCount = pageCount;
            search.Page = pageNumber;

            var productsInUserShoppingCart = this.GetUserShoppingCard(userId)?.Purchases.Count ?? 0;
            var result = new ProductsListVM
            {
                Products = productVms,
                SearchProductsBM = search,
                UserProductsInShoppingCart = productsInUserShoppingCart
            };

            return result;
        }

        public ShoppingCart GetUserShoppingCard(string userId)
        {
            var shoppingCart = this.Context.ShoppingCarts.All()
                .FirstOrDefault(c => string.Equals(c.UserId, userId));

            return shoppingCart;
        }

        public ShoppingCardVM GetUserShoppingCardVM(string userId, SearchProductsBM searchProductsBM)
        {
            var shopingCard = this.GetUserShoppingCard(userId);
            var shoppingCardVM = Mapper.Map<ShoppingCardVM>(shopingCard);

            foreach (var purchase in shoppingCardVM.Purchases)
            {
                purchase.Price = purchase.Product.Price;
                purchase.Value = purchase.Quantity * purchase.Price;
            }

            shoppingCardVM.Total = shoppingCardVM.Purchases.Sum(p => p.Value);
            shoppingCardVM.SearchProductsBM = searchProductsBM;
            shoppingCardVM.IsAnyPurchaseOnStock = shoppingCardVM.Purchases
                .Any(pu => pu.Quantity > 0 && pu.Product.Quantity >= pu.Quantity);

            return shoppingCardVM;
        }

        public ShoppingCardVM GetUserFinishOrderVM(string userId, SearchProductsBM searchProductsBM)
        {
            var finishOrderVM = this.GetUserShoppingCardVM(userId, searchProductsBM);
            if (finishOrderVM == null)
            {
                return null;
            }

            var purchasesToBuy = new List<PurchaseVM>();
            foreach (var purchase in finishOrderVM.Purchases)
            {
                if (purchase.Quantity > 0 && purchase.Product.Quantity >= purchase.Quantity)
                {
                    purchasesToBuy.Add(purchase);
                }
            }

            finishOrderVM.Purchases = purchasesToBuy;

            finishOrderVM.Total = purchasesToBuy.Sum(p => p.Value);

            if (finishOrderVM.Total == 0)
            {
                return null;
            }

            return finishOrderVM;
        }

        public bool AddProductToUserShoppingCard(string userId, int productId)
        {
            var shoppingCard = this.Context.Users.Find(userId)?.ShoppingCart;
            if (shoppingCard == null)
            {
                var user = this.Context.Users.Find(userId);
                if (user == null)
                {
                    return false;
                }

                shoppingCard = new ShoppingCart
                {
                    UserId = user.Id,
                    DeliveryAddress = user.Address
                };
                this.Context.ShoppingCarts.Add(shoppingCard);
                this.Context.SaveChanges();
                user.ShoppingCart = shoppingCard;
                this.Context.SaveChanges();
            }

            var product = this.Context.Products.Find(productId);
            if (product == null || product.Quantity == 0 || shoppingCard.Purchases.Any(p => p.Product.Id == productId))
            {
                return false;
            }

            var purchase = new Purchase
            {
                Quantity = 1,
                Price = product.Price,
                Product = product
            };

            shoppingCard.Purchases.Add(purchase);

            this.Context.SaveChanges();

            return true;
        }

        public bool UpdatePurchaseToUserShoppingCard(string userId, int purchaseId, int quantity)
        {
            var shoppingCard = this.Context.Users.Find(userId)?.ShoppingCart;
            var puchase = shoppingCard?.Purchases.FirstOrDefault(p => p.Id == purchaseId);
            if (puchase == null)
            {
                return false;
            }

            if (quantity == 0)
            {
                this.Context.Purchases.Remove(puchase);
                this.Context.SaveChanges();
                return true;
            }

            if (quantity > puchase.Product.Quantity)
            {
                quantity = puchase.Product.Quantity;
            }

            puchase.Quantity = quantity;
            this.Context.SaveChanges();

            return true;
        }

        public bool FinishOrder(string userId, ShoppingCardVM finishOrderVM)
        {
            var user = this.Context.Users.Find(userId);
            if (user?.ShoppingCart == null || !string.Equals(user.Id, finishOrderVM.UserId))
            {
                return false;
            }

            var invoice = new Order
            {
                Date = DateTime.Now,
                DeliveryAddress = finishOrderVM.DeliveryAddress,
                User = user,
                UserId = userId
            };

            this.Context.Orders.Add(invoice);
            this.Context.SaveChanges();

            foreach (var purchaseVM in finishOrderVM.Purchases)
            {
                var purchase = user.ShoppingCart.Purchases.FirstOrDefault(p => p.Id == purchaseVM.Id);
                if (purchase == null)
                {
                    return false;
                }

                if (purchase.Product.Quantity < purchase.Quantity || purchase.Quantity <= 0)
                {
                    return false;
                }

                purchase.Price = purchaseVM.Price;
                user.ShoppingCart.Purchases.Remove(purchase);
                invoice.Purchases.Add(purchase);
                purchase.Product.Quantity -= purchase.Quantity;
            }

            this.Context.SaveChanges();

            return true;
        }
    }
}