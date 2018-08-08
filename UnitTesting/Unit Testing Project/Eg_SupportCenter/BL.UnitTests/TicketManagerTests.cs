using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC.BL;
using SC.BL.Domain;
using SC.DAL;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Diagnostics;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace BL.UnitTests
{
    [TestClass]
    public class TicketManagerTests
    {
        //Create global repository
        private static ITicketRepository ticketRepository;
        private static Mock<ITicketRepository> mockedRepository;

        [TestInitialize]
        public void Initialize()
        {
            //stub with hardcoded values to mitigate database interaction
            ticketRepository = new TicketRepositoryHC();

            //Mock using Moq
            mockedRepository = new Mock<ITicketRepository>();
            //ticketRepository = mockedRepository.Object;
        }

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

            //When mocking
            //mockedRepository.Setup(x => x.ReadTicket(It.IsAny<int>())).Returns(new Ticket { TicketNumber = 1, AccountId = 1, Text = "I am a ticket", Responses = new List<TicketResponse>()});

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

            //When mocking
            //mockedRepository.Setup(x => x.CreateTicket(It.IsAny<Ticket>())).Returns<Ticket>(x => x);

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
            //implicite private validation method testing
            //avoiding reflection
        }

        //testing private validation method using reflection
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void Validate_TicketResponseIsInvalid_ReturnsValidationException()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository);
            Ticket t = new Ticket { AccountId = 1, Text = "How do I test a private method in C#?", TicketNumber = 5 };
            TicketResponse tr = new TicketResponse { Ticket = t, IsClientResponse = false, Date = DateTime.Now };

            //reflection
            MethodInfo methodInfo = typeof(TicketManager).GetMethod("Validate", BindingFlags.NonPublic | BindingFlags.Instance,
            null, new Type[] { typeof(TicketResponse) }, null);
            object[] parameters = {tr};
            //Act
            methodInfo.Invoke(ticketManager, parameters);

            //Assert
            //assertion happens using attribute added to method
        }

        //testing private method using --PrivateObject--
        [TestMethod]
        public void Validate_TicketIsInvalid_ReturnsValidationException()
        {
            //Arrange
            PrivateObject ticketManager = new PrivateObject(new TicketManager(ticketRepository));
            Ticket t = new Ticket { AccountId = 1, Text = "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" };

            //Act
            //act happens using delegation when asserting

            //Assert => Assert.ThrowsException does not allow derived exceptions.
            Assert.ThrowsException<TargetInvocationException>(() => ticketManager.Invoke("Validate", t));
        }

        [TestMethod]
        public void ChangeTicket_TextIsChanged_TicketHasBeenUpdated()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository);
            Ticket t1 = ticketRepository.ReadTicket(1); // GET : ticket 

            //Act
            t1.Text = "Unit testing the changed ticket";
            ticketManager.ChangeTicket(t1); // Update the repo with new values
            var result = ticketManager.GetTicket(1); // Get the ticket back

            //Assert
            Assert.AreEqual(result.Text, "Unit testing the changed ticket"); // Check if ticket has changed
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException), "Ticket not valid!")]
        public void ChangeTicket_ChangedTextInvalid_ReturnsValidationException()
        {
            //Arrange
            TicketManager ticketManager = new TicketManager(ticketRepository);
            Ticket t1 = ticketRepository.ReadTicket(1);

            //Act
            t1.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            ticketManager.ChangeTicket(t1); // ChangeTicket method contains a validator, which will set the ticket to invalid
            var result = ticketManager.GetTicket(1);

            //Assert
            //assertion happens using attribute added to method
        }
    }
}
