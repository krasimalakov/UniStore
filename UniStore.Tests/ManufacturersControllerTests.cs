using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UniStore.Tests
{
    using System.Security.Principal;
    using System.Web.Mvc;
    using App.Controllers;
    using Data.UnitOfWork;
    using Models.BindingModels.Manufacturer;
    using Models.EntityModels;
    using Services.Interfaces;
    using Moq;
    using Services.Implementation;
    using TestStack.FluentMVCTesting;

    [TestClass]
    public class ManufacturersControllerTests
    {
        private IManufacturersService service;
        private ManufacturersController controller;
        public void InitTestWithAdmin()
        {
            var mockDb = new Mock<IUniStoreContext>();
            var mockService = new Mock<IManufacturersService>();
            mockService.Setup(s => s.AddManufacturer(It.IsAny<AddManufacturerBM>()));

            this.service = mockService.Object;
            // create mock principal
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("admin");
            mockPrincipal.Setup(p => p.IsInRole("Administrator")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // create controller
            this.controller = new ManufacturersController(mockDb.Object, this.service);
        }

        [TestMethod]
        public void Call_All_ShouldReturnDefaltView()
        {
            this.InitTestWithAdmin();

            this.controller
                .WithCallTo(c => c.All())
                .ShouldRenderDefaultView();

        }

        [TestMethod]
        public void Call_Add_WithValiData_ShouldReturnRedirect()
        {
            this.InitTestWithAdmin();

            var manufacturerBM = new AddManufacturerBM() {Name="IBM"};

            this.controller
                .WithCallTo(c => c.Add(manufacturerBM))
                .ShouldRedirectTo(c=>c.All);

        }

        [TestMethod]
        public void Call_Add_WithInValidData_ShouldReturnRedirect()
        {
            this.InitTestWithAdmin();

            var manufacturerBM = new AddManufacturerBM() { Name = "I" };

            this.controller.ModelState.AddModelError("Name", "Name must be at least 3 symbols long");

            this.controller
                .WithCallTo(c => c.Add(null))
                .ShouldRenderDefaultView();

        }
    }
}
