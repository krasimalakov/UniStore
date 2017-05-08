namespace UniStore.Services.Implementation
{
    using System.Linq;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Models;
    using Models.ViewModels.Manufacturer;
    using Models.ViewModels.Order;

    public class OrdersService : BaseService, IOrdersService
    {
        public OrdersService(IUniStoreContext context) : base(context)
        {
        }

        public bool IsExistUser(string username)
        {
            return this.Context.Users.All().Any(u => string.Equals(u.UserName, username));
        }

        public OrdersListVM GetOrdersVM(Pagination pagination)
        {
            const int PageSize = 3;

            var pageNumber = pagination.Page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var username = pagination.Search;
            var orders = this.Context.Orders.All()
                .Where(
                    o => string.IsNullOrEmpty(username) ||
                         string.Equals(o.User.UserName, username))
                .OrderByDescending(o=>o.Id)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(Mapper.Map<OrderVM>)
                .ToArray();

            var ordersCount = this.Context.Orders.All()
                .Count(
                    o => string.IsNullOrEmpty(username) ||
                         string.Equals(o.User.UserName, username));

            var pageCount = ordersCount / PageSize + (ordersCount % PageSize > 0 ? 1 : 0);
            var ordersListVM = new OrdersListVM()
            {
                Orders = orders,
                Pagination = new Pagination
                {
                    Page = pageNumber,
                    PageCount = pageCount,
                    Search = pagination.Search
                }
            };

            return ordersListVM;
        }

        public OrderVM GetOrderVM(int orderId)
        {
            var order = this.Context.Orders.Find(orderId);
            if (order == null)
            {
                return null;
            }

            var orderVM = Mapper.Map<OrderVM>(order);

            return orderVM;
        }
    }
}