namespace UniStore.Services.Interfaces
{
    using Models;
    using Models.ViewModels.Order;

    public interface IOrdersService
    {
        bool IsExistUser(string username);

        OrdersListVM GetOrdersVM(Pagination pagination);
    }
}