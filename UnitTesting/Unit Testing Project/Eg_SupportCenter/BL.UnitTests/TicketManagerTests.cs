using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.BL;
using SC.BL.Domain;
using SC.DAL;
using System.ComponentModel.DataAnnotations;

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
            Assert.AreEqual(result.Text, response);
            Assert.AreEqual(result.Ticket.TicketNumber, ticketNumber);
            Assert.AreEqual(result.IsClientResponse, isClientResponse);
            Assert.IsInstanceOfType(result, typeof(TicketResponse));
        }

        [TestMethod]
        public void AddTicket_TicketIsValid_ReturnsTicket()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository);

            //Act
            var result = ticketManager.AddTicket(1, "I can't run my unit test, is it broken?");

            //Assert
            Assert.IsInstanceOfType(result, typeof(Ticket));
            Assert.AreEqual(result.AccountId, 1);
            Assert.AreEqual(result.State, TicketState.Open);
            Assert.AreEqual(result.Text, "I can't run my unit test, is it broken?");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException), "Ticket not valid!")]
        public void AddTicket_TicketIsinvalid_ReturnsValidationException()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository);

            //Act
            var result = ticketManager.AddTicket(1, "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");

            //Assert
            //assertion happens using attribute added to method
        }
    }
}
