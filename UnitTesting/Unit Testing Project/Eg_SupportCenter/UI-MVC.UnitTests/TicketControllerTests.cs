using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.UI.Web.MVC.Controllers;
using System.Web.Mvc;
using Moq;
using SC.BL.Domain;
using SC.BL;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace UI_MVC.UnitTests
{
    [TestClass]
    public class TicketControllerTests
    {
        //global data sets to use when mocking
        private static ITicketManager mgr = new TicketManager();

        // GET: Tickets
        [TestMethod]
        public void Index() // ActionResult instead of void?
        {
            IEnumerable<Ticket> tickets = mgr.GetTickets();
            
        }

        // Get: Tickets/Details
        [TestMethod]
        public void Details_ShowTicketDetails_ReturnTicketDetails()
        {
            // Arrange
            //Mock<Ticket> ticket = mgr.GetTicket(2);   
            //var controller = new TicketController();

            // Act
            //var result = controller.Details(2) as ViewResult;

            // Assert
            //Assert.AreEqual(ticket, result.ViewName);
        }
    }
}
