using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.UI.Web.MVC.Controllers;
using System.Web.Mvc;
using Moq;
using SC.BL.Domain;
using SC.BL;
using System.Linq;
using System.Linq.Expressions;
using SC.DAL;
using SC.UI.Web.MVC.Models;

namespace UI_MVC.UnitTests
{
    [TestClass]
    public class TicketControllerTests
    {
        //global controller
        private TicketController controller;

        [TestInitialize]
        public void Initialize()
        {
            controller = new TicketController(new TicketManager(new TicketRepositoryHC()));
        }

        //ctrl R, ctrl + T to debug single method
        [TestMethod]
        public void Details_ShowDetails_ReturnsDetailsView()
        {
            //Arrange

            //Act
            var result = controller.Details(1) as ViewResult;

            //Assert
            Ticket t = (Ticket)result.ViewData.Model;

            Assert.AreEqual("Details", result.ViewName);
            Assert.AreEqual(1, t.TicketNumber);
        }

        [TestMethod]
        public void Index_ReturnsViewIndex()
        {
            //Arrange

            //Act
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Create_ReturnsViewCreate()
        {
            //Arrange

            //Act
            var result = controller.Create() as ViewResult;

            //Assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void Create_ModelStateIsValid_ReturnsRedirectToDetailsView()
        {
            //Arrange
            CreateTicketVM ticketVM = new CreateTicketVM { AccId = 1, Problem = "Cannot find webbrowser" };

            //Act
            var result = (RedirectToRouteResult)controller.Create(ticketVM);

            //Assert
            Assert.AreEqual("Details", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_ModelStateIsNotValid_ReturnsCreateView()
        {
            //Arrange
            CreateTicketVM ticketVM = new CreateTicketVM { AccId = 1, Problem = "test" };

            //Act
            controller.ModelState.AddModelError("Problem", "Problem need to be at least 50 characters"); //create fake error to set ModelState.IsValid to false
            var result = controller.Create(ticketVM) as ViewResult;

            //Assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void Edit_EditTicketOne_ReturnsViewEdit()
        {
            //Arrange

            //Act
            var result = controller.Edit(1) as ViewResult;

            //Assert
            Ticket t = (Ticket)result.ViewData.Model;

            Assert.AreEqual("Edit", result.ViewName);
            Assert.AreEqual(1, t.TicketNumber);
        }

        [TestMethod]
        public void Edit_ModelStateIsValid_ReturnsRedirectToIndex()
        {
            //Arrange
            Ticket t = new Ticket() { AccountId = 1, TicketNumber = 1, Text = "Problem solved" };
            //controller.ModelState["Problem"].Errors.Clear();

            //Act
            var result = (RedirectToRouteResult)controller.Edit(1, t);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Edit_ModelStateIsNotValid_ReturnsEditView()
        {
            //Arrange
            Ticket t = new Ticket() { AccountId = 1, TicketNumber = 1, Text = "Problem solved" };
            controller.ModelState.AddModelError("Text", "Description can not be empty");

            //Act
            var result = controller.Edit(1, t) as ViewResult;

            //Assert
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod]
        public void Deletee_ReturnsDeleteView()
        {
            //Arrange

            //Act
            var result = controller.Delete(1) as ViewResult;

            //Assert
            Ticket t = (Ticket)result.Model;

            Assert.AreEqual("Delete", result.ViewName);
            Assert.AreEqual(1, t.TicketNumber);
        }

        [TestMethod]
        public void Delete_DeleteExistingTicket_ReturnsRedirectToIndex()
        {
            //Arrange

            //Act
            var result = (RedirectToRouteResult)controller.Delete(1, new FormCollection());

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_DeleteNonexistingTicket_ReturnsRedirectToIndex()
        {
            //Arrange

            //Act
            var result = (RedirectToRouteResult)controller.Delete(1000, new FormCollection());

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}