namespace UniStore.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.UnitOfWork;
    using Models.EntityModels;

    public class BaseController : Controller
    {
        public BaseController(IUniStoreContext context)
        {
            this.Context = context;
        }

        public BaseController(IUniStoreContext context, User user)
            : this(context)
        {
            this.UserProfile = user;
        }

        private IUniStoreContext Context { get; }

        protected User UserProfile { get; set; }

        protected override IAsyncResult BeginExecute(
            RequestContext requestContext,
            AsyncCallback callback,
            object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.Context.Users.All().FirstOrDefault(u => u.UserName == username);
                this.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}