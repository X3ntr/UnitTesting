using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.BL;
using SC.BL.Domain;
using SC.DAL;

namespace BL.UnitTests
{
    [TestClass]
    public class TicketManagerTests
    {
        //Create global hardcoded repository to mitigate database interaction
        private static TicketRepositoryHC ticketRepository = new TicketRepositoryHC();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Ticketnumber '0' not found!")]
        public void AddTicketResponse_TicketIsInvalid_ReturnsArgumentException()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository); //using overloaded constructor

            //Act
            var result = ticketManager.AddTicketResponse(0, "This ticket is not valid", false);

            //Assert
            //assertion happens using attribute added to method
        }

        [TestMethod]
        public void AddTicketResponse_TicketIsValid_ReturnsNewTicketResponse()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository); //using overloaded constructor
            string response = "This ticket is being processed";
            int ticketNumber = 1;
            bool isClientResponse = false;

            //Act
            var result = ticketManager.AddTicketResponse(ticketNumber, response, isClientResponse);

            //Assert
            Assert.IsTrue(result.Text.Equals(response));
            Assert.IsTrue(result.Ticket.TicketNumber.Equals(ticketNumber));
            Assert.IsTrue(result.IsClientResponse.Equals(isClientResponse));
            Assert.IsInstanceOfType(result, typeof(TicketResponse));
        }
    }
}
