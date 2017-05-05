namespace UniStore.App.Attributes
{
    using System.Web.Mvc;
    using Models.Enums;

    public class AuthorizeInRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeInRoleAttribute(params AppRole[] roles)
        {
            this.Roles = string.Join(", ", roles);
        }
    }
}