using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SC.BL.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using SC.DAL.EF;
using Moq;

namespace DAL.UnitTests
{
    [TestClass]
    public class TicketRepositoryTests
    {
        //set SupportCenterDbContext DbSets to virtual to allow mocking
        //set Ticket, TicketResponse relations to virtual

        //create global mocked context to use for testing
        private static SupportCenterDbContext ctx = Mock();

        //create Mock() method to mock DbContext and seed with dummy data
        private static SupportCenterDbContext Mock()
        {
            //create dummy data
            #region
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

            //initialize fakes
            #region
            var tickets = new List<Ticket>
            {
                //add data
                t1,
                t2
                
            }.AsQueryable();

            var hardwareTickets = new List<HardwareTicket>
            {
                //add data
                ht1

            }.AsQueryable();

            var ticketResponses = new List<TicketResponse>
            {
                //add data
                t1r1,
                t1r2,
                t1r3,
                t2r1

            }.AsQueryable();
            #endregion

            //create mock DbSets
            var mockTickets = new Mock<DbSet<Ticket>>();
            var mockHarwareTickets = new Mock<DbSet<HardwareTicket>>();
            var mockTicketResponses = new Mock<DbSet<TicketResponse>>();

            //set properties
            #region
            mockTickets.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(tickets.Provider);
            mockTickets.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(tickets.Expression);
            mockTickets.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(tickets.ElementType);
            mockTickets.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(() => tickets.GetEnumerator());

            mockHarwareTickets.As<IQueryable<HardwareTicket>>().Setup(m => m.Provider).Returns(hardwareTickets.Provider);
            mockHarwareTickets.As<IQueryable<HardwareTicket>>().Setup(m => m.Expression).Returns(hardwareTickets.Expression);
            mockHarwareTickets.As<IQueryable<HardwareTicket>>().Setup(m => m.ElementType).Returns(hardwareTickets.ElementType);
            mockHarwareTickets.As<IQueryable<HardwareTicket>>().Setup(m => m.GetEnumerator()).Returns(() => hardwareTickets.GetEnumerator());

            mockTicketResponses.As<IQueryable<TicketResponse>>().Setup(m => m.Provider).Returns(ticketResponses.Provider);
            mockTicketResponses.As<IQueryable<TicketResponse>>().Setup(m => m.Expression).Returns(ticketResponses.Expression);
            mockTicketResponses.As<IQueryable<TicketResponse>>().Setup(m => m.ElementType).Returns(ticketResponses.ElementType);
            mockTicketResponses.As<IQueryable<TicketResponse>>().Setup(m => m.GetEnumerator()).Returns(() => ticketResponses.GetEnumerator());
            #endregion

            //create mock context
            var mockContext = new Mock<SupportCenterDbContext>();

            //set properties
            mockContext.Setup(m => m.Tickets).Returns(mockTickets.Object);
            mockContext.Setup(m => m.TicketResponses).Returns(mockTicketResponses.Object);
            mockContext.Setup(m => m.HardwareTickets).Returns(mockHarwareTickets.Object);

            //return mocked DbContext
            return mockContext.Object;
        }


        [TestMethod]
        public void ReadTickets_ReadAllTickets_ReturnsIEnumerableTicketsContainingAllTickets()
        {
            //Arrange
            TicketRepository ticketRepository = new TicketRepository(ctx);

            //Act
            var result = ticketRepository.ReadTickets();

            //Assert
            Assert.AreEqual(result.Count(), ctx.Tickets.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Ticket>));
        }
    }
}
