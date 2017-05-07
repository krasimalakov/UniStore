namespace UniStore.Models.ViewModels.User
{
    using System.Web.Mvc;

    public class EditUserVM : UserFullVM
    {
        public SelectList AppRoles { get; set; }

        public string NewRole { get; set; }
    }
}