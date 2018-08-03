using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SC.BL.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using SC.DAL.EF;
using Moq;
using System.Linq.Expressions;

namespace DAL.UnitTests
{
    [TestClass]
    public class TicketRepositoryTests
    {
        //set SupportCenterDbContext DbSets to virtual to allow mocking
        //set Ticket, TicketResponse relations to virtual

        //created extension methods => DbSetMocking.cs to mock DbSet more easily

        //global data sets to use when mocking
        private static IEnumerable<Ticket> tickets;
        private static IEnumerable<HardwareTicket> hardwareTickets;
        private static IEnumerable<TicketResponse> ticketResponses;

        //test initializer to create dummy data for mocking
        [TestInitialize]
        public void Initialize()
        {
            #region create dummy data
            Ticket t1 = new Ticket() { TicketNumber = 1, AccountId = 1, Text = "Ik kan mij niet aanmelden op de webmail", DateOpened = new DateTime(2012, 9, 9, 13, 5, 59), State = TicketState.Closed, Responses = new List<TicketResponse>() };

            TicketResponse t1r1 = new TicketResponse() { Id = 1, Ticket = t1, Text = "Account is geblokkeerd", Date = new DateTime(2012, 9, 9, 13, 24, 48), IsClientResponse = false };
            t1.Responses.Add(t1r1);

            TicketResponse t1r2 = new TicketResponse() { Id = 2, Ticket = t1, Text = "Account terug in orde en nieuw paswoord ingesteld", Date = new DateTime(2012, 9, 9, 13, 29, 11), IsClientResponse = false };
            t1.Responses.Add(t1r2);

            TicketResponse t1r3 = new TicketResponse() { Id = 3, Ticket = t1, Text = "Aanmelden gelukt en paswoord gewijzigd", Date = new DateTime(2012, 9, 10, 7, 22, 36), IsClientResponse = true };
            t1.Responses.Add(t1r3);

            Ticket t2 = new Ticket() { TicketNumber = 2, AccountId = 1, Text = "Geen internetverbinding", DateOpened = new DateTime(2012, 11, 5, 9, 45, 13), State = TicketState.Answered, Responses = new List<TicketResponse>() };

            TicketResponse t2r1 = new TicketResponse() { Id = 4, Ticket = t2, Text = "Controleer of de kabel goed is aangesloten", Date = new DateTime(2012, 11, 5, 11, 25, 42), IsClientResponse = false, };
            t2.Responses.Add(t2r1);

            HardwareTicket ht1 = new HardwareTicket() { TicketNumber = 3, AccountId = 2, Text = "Blue screen!", DateOpened = new DateTime(2012, 12, 14, 19, 15, 32), State = TicketState.Open, DeviceName = "PC-123456" };
            #endregion

            #region initialize fakes
            tickets = new List<Ticket>
                {
                    //add data
                    t1,
                    t2

                }.AsQueryable();

            hardwareTickets = new List<HardwareTicket>
                {
                    //add data
                    ht1

                }.AsQueryable();

            ticketResponses = new List<TicketResponse>
                {
                    //add data
                    t1r1,
                    t1r2,
                    t1r3,
                    t2r1

                }.AsQueryable();
            #endregion
        }

        [TestMethod]
        public void ReadTickets_ReadAllTickets_ReturnsIEnumerableTicketsContainingAllTickets()
        {
            //Arrange
            var mockContext = new Mock<SupportCenterDbContext>();
            mockContext.Setup(t => t.Tickets).ReturnsDbSet(tickets);

            TicketRepository ticketRepository = new TicketRepository(mockContext.Object);

            //Act
            var result = ticketRepository.ReadTickets();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), mockContext.Object.Tickets.Count());
        }

        [TestMethod]
        public void ReadTicket_ReadTicketOne_ReturnsTicketOne()
        {
            //Arrange
            var mockContext = new Mock<SupportCenterDbContext>();
            mockContext.Setup(t => t.Tickets).ReturnsDbSet(tickets);
            mockContext.Setup(t => t.Tickets.Find(It.IsAny<int>())).Returns<int>(x => mockContext.Object.Tickets.Find(x));

            TicketRepository ticketRepository = new TicketRepository(mockContext.Object);

            //Act
            var result = ticketRepository.ReadTicket(1);

            //Assert
            mockContext.Verify(x => x.Tickets.Find(It.IsAny<int>()), Times.Once());
            //Assert.IsInstanceOfType(result, typeof(Mock<Ticket>));
        }

        [TestMethod]
        public void CreateTicket_CreateNewTicket_ReturnsTicketAndSavesToContext()
        {
            //Arrange
            var mockContext = new Mock<SupportCenterDbContext>();
            mockContext.Setup(x => x.Tickets).ReturnsDbSet(tickets);
            mockContext.Setup(x => x.Tickets.Add(It.IsAny<Ticket>())).Returns<Ticket>(x => x);

            TicketRepository ticketRepository = new TicketRepository(mockContext.Object);
            Ticket t = new Ticket { AccountId = 1, DateOpened = DateTime.Now, State = TicketState.Open, Text = "I am a new ticket", Responses = new List<TicketResponse>() };

            //Act
            var result = ticketRepository.CreateTicket(t);

            //Assert
            Assert.IsInstanceOfType(result, typeof(Ticket));
            mockContext.Verify(x => x.Tickets.Add(It.IsAny<Ticket>()), Times.Once());
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }
    }
}
