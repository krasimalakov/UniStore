namespace UniStore.Services.Interfaces
{
    using System.Collections.Generic;
    using Models.BindingModels;
    using Models.BindingModels.User;
    using Models.EntityModels;
    using Models.ViewModels.User;

    public interface IAdminService
    {
        UserVM GetUserVM(string username = null, User user = null);

        IEnumerable<UserVM> GetAllUserVMs();

        UserFullVM GetUserFullVM(string username);

        EditUserVM GetEditUserVM(string username);

        void UpdateUserData(EditUserBM userBM, bool isAdmin);

        void RemoveUser(string username, string cuurentUserName);
    }
}