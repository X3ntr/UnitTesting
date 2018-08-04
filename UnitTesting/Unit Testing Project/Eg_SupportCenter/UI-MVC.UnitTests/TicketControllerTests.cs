using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.UI.Web.MVC.Controllers;
using System.Web.Mvc;
using Moq;
using SC.BL.Domain;
using SC.BL;
using System.Linq;
using System.Linq.Expressions;

namespace UI_MVC.UnitTests
{
    [TestClass]
    public class TicketControllerTests
    {
        //global data sets to use when mocking
        //private static ITicketManager mgr = new TicketManager();

        [TestMethod]
        public void Details_ShowDetails_ReturnsDetailsView()
        {
            //problem with system.web.mvc / a dependency file not found error BUG
            //Arrange
            //Mock<Ticket> ticket = new Mock<Ticket>(); // Need a mock ticket
            var controller = new TicketController(); // the Controller to test

            //Act
            var result = controller.Details(2) as ViewResult; 

            //Assert
            Assert.AreEqual("Details", result.ViewName); // Returns view name
        }
    }
}
