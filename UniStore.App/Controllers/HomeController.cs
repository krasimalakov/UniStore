namespace UniStore.App.Controllers
{
    using System;
    using System.Web.Mvc;
    using Data.UnitOfWork;
    using Models.EntityModels;
    using Models.Enums;

    public class HomeController : BaseController
    {
        public HomeController(IUniStoreContext context) : base(context)
        {
        }

        public HomeController(IUniStoreContext context, User user) : base(context, user)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated &&
                this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator)))
            {
                return this.RedirectToAction("All", "Departments");
            }

            return this.RedirectToAction("Store", "Store");
        }

        [HttpGet]
        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }
    }
}