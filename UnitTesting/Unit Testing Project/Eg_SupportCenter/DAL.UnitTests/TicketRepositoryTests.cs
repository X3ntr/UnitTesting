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
            var mockTickets = new Mock<DbSet<Ticket>>();
            var mockHarwareTickets = new Mock<DbSet<HardwareTicket>>();
            var mockTicketResponses = new Mock<DbSet<TicketResponse>>();

            var mockContext = new Mock<SupportCenterDbContext>();

            mockContext.Setup(m => m.Tickets).Returns(mockTickets.Object);
            mockContext.Setup(m => m.TicketResponses).Returns(mockTicketResponses.Object);
            mockContext.Setup(m => m.HardwareTickets).Returns(mockHarwareTickets.Object);

            TicketRepository ticketRepository = new TicketRepository(mockContext.Object);

            //Act
            var result = ticketRepository.ReadTickets();

            //Assert
            Assert.AreEqual(result.Count(), mockContext.Object.Tickets.Count());
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Ticket>));
        }
    }
}
