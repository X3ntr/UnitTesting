using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.UI.Web.MVC.Controllers;
using System.Web.Mvc;
using Moq;
using SC.BL.Domain;
using System.Linq;
using System.Linq.Expressions;

namespace UI_MVC.UnitTests
{
    [TestClass]
    public class TicketControllerTests
    {
        [TestMethod]
        public void TestDetailsView()
        {
            //Arrange
            Mock<Ticket> ticket = new Mock<Ticket>(); // Need a mock ticket, WIP
            var controller = new TicketController(); // the Controller to test

            //Act
            var result = controller.Details(2) as ViewResult; // A random ticket ID

            //Assert
            Assert.AreEqual(ticket, result.ViewName); // Requires a ticket to return
        }
    }
}
