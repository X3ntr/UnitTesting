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

        [TestMethod]
        public void ReadTickets_ReadAllTickets_ReturnsIEnumerableTicketsContainingAllTickets()
        {
            //Arrange
            var tickets = new List<Ticket>
            {
                //add data
            }.AsQueryable();

            var hardwareTickets = new List<HardwareTicket>
            {
                //add data
            }.AsQueryable();

            var ticketResponses = new List<TicketResponse>
            {
                //add data
            }.AsQueryable();

            var mockTickets = new Mock<DbSet<Ticket>>();
            var mockHarwareTickets = new Mock<DbSet<HardwareTicket>>();
            var mockTicketResponses = new Mock<DbSet<TicketResponse>>();

            var mockContext = new Mock<SupportCenterDbContext>();

            mockContext.Setup(m => m.Tickets).Returns(mockTickets.Object);
            mockContext.Setup(m => m.TicketResponses).Returns(mockTicketResponses.Object);
            mockContext.Setup(m => m.HardwareTickets).Returns(mockHarwareTickets.Object);

            mockContext.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(tickets.Provider);
            mockContext.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(tickets.Expression);
            mockContext.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(tickets.ElementType);
            mockContext.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(tickets.GetEnumerator());

            mockContext.As<IQueryable<HardwareTicket>>().Setup(m => m.Provider).Returns(hardwareTickets.Provider);
            mockContext.As<IQueryable<HardwareTicket>>().Setup(m => m.Expression).Returns(hardwareTickets.Expression);
            mockContext.As<IQueryable<HardwareTicket>>().Setup(m => m.ElementType).Returns(hardwareTickets.ElementType);
            mockContext.As<IQueryable<HardwareTicket>>().Setup(m => m.GetEnumerator()).Returns(hardwareTickets.GetEnumerator());

            mockContext.As<IQueryable<TicketResponse>>().Setup(m => m.Provider).Returns(ticketResponses.Provider);
            mockContext.As<IQueryable<TicketResponse>>().Setup(m => m.Expression).Returns(ticketResponses.Expression);
            mockContext.As<IQueryable<TicketResponse>>().Setup(m => m.ElementType).Returns(ticketResponses.ElementType);
            mockContext.As<IQueryable<TicketResponse>>().Setup(m => m.GetEnumerator()).Returns(ticketResponses.GetEnumerator());

            TicketRepository ticketRepository = new TicketRepository(mockContext.Object);

            //Act
            var result = ticketRepository.ReadTickets();

            //Assert
            Assert.AreEqual(result.Count(), mockContext.Object.Tickets.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Ticket>));
        }
    }
}
