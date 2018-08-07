using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.UI.Web.MVC.Controllers;
using System.Web.Mvc;
using Moq;
using SC.BL.Domain;
using SC.BL;
using System.Linq;
using System.Linq.Expressions;
using SC.DAL;

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
            Ticket t = (Ticket) result.ViewData.Model;

            Assert.AreEqual("Details", result.ViewName);
            Assert.AreEqual(1, t.TicketNumber);
        }
    }
}
