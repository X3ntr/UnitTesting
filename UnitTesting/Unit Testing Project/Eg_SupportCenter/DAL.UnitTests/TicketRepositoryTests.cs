using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SC.BL.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using NSubstitute;
using SC.DAL.EF;

namespace DAL.UnitTests
{
    [TestClass]
    public class TicketRepositoryTests
    {
        private static MockContext ctx = Mock();

        //Mock DbContext
        private static MockContext Mock()
        {
            //create DbContext derived class for persisting
            //create interface for this class

            //Create tickets
            #region create tickets
            Ticket t1 = new Ticket
            {
                TicketNumber = 1,
                AccountId = 1,
                State = TicketState.Closed,
                DateOpened = new DateTime(2012, 9, 9, 13, 5, 59),
                Text = "Ik kan mij niet aanmelden op de webmail",
                Responses = new List<TicketResponse>()
            };

            Ticket t2 = new Ticket
            {
                TicketNumber = 2,
                AccountId = 1,
                DateOpened = new DateTime(2012, 11, 5, 9, 45, 13),
                State = TicketState.Open,
                Text = "Geen internet verbinding",
                Responses = new List<TicketResponse>()
            };

            HardwareTicket ht1 = new HardwareTicket
            {
                TicketNumber = 3,
                AccountId = 2,
                DateOpened = new DateTime(2012, 12, 14, 19, 15, 32),
                State = TicketState.Open,
                Text =
                "blue screen1",
                DeviceName = "PC-123456"
            };
            #endregion


            //create ticket responses
            #region create ticket responses
            TicketResponse t1r1 = new TicketResponse() {
                Id = 1,
                Ticket = t1,
                Text = "Account is geblokkeerd",
                Date = new DateTime(2012, 9, 9, 13, 24, 48),
                IsClientResponse = false
            };

            TicketResponse t1r2 = new TicketResponse()
            {
                Id = 2,
                Ticket = t1,
                Text = "Account terug in orde en nieuw paswoord ingesteld",
                Date = new DateTime(2012, 9, 9, 13, 29, 11),
                IsClientResponse = false
            };

            TicketResponse t1r3 = new TicketResponse()
            {
                Id = 3,
                Ticket = t1,
                Text = "Aanmelden gelukt en paswoord gewijzigd",
                Date = new DateTime(2012, 9, 10, 7, 22, 36),
                IsClientResponse = true
            };

            TicketResponse t2r1 = new TicketResponse()
            {
                Id = 4,
                Ticket = t2,
                Text = "Controleer of de kabel goed is aangesloten",
                Date = new DateTime(2012, 11, 5, 11, 25, 42),
                IsClientResponse = false
            };

            //add responses to tickets
            t1.Responses.Add(t1r1);
            t1.Responses.Add(t1r2);
            t1.Responses.Add(t1r3);
            t2.Responses.Add(t2r1);
            #endregion


            //create fakes to use when mocking DbContext
            #region create gakes to use when mocking DbContext
            IQueryable<Ticket> tickets = new List<Ticket>
            {
                t1,
                t2
            }.AsQueryable();

            IQueryable<TicketResponse> ticketResponses = new List<TicketResponse>
            {
                t1r1,
                t1r2,
                t1r3,
                t2r1

            }.AsQueryable();

            IQueryable<HardwareTicket> hardwareTickets = new List<HardwareTicket>
            {
                ht1
            }.AsQueryable();
            #endregion

            //mock DbContext
            #region mock DbContext
            //create mock DBSets
            IDbSet<Ticket> Tickets = Substitute.For<IDbSet<Ticket>>();
            IDbSet<HardwareTicket> HardwareTickets = Substitute.For<IDbSet<HardwareTicket>>();
            IDbSet<TicketResponse> TicketResponses = Substitute.For<IDbSet<TicketResponse>>();

            //set substitute properties
            Tickets.Provider.Returns(tickets.Provider);
            Tickets.Expression.Returns(tickets.Expression);
            Tickets.ElementType.Returns(tickets.ElementType);
            Tickets.GetEnumerator().Returns(tickets.GetEnumerator());

            HardwareTickets.Provider.Returns(hardwareTickets.Provider);
            HardwareTickets.Expression.Returns(hardwareTickets.Expression);
            HardwareTickets.ElementType.Returns(hardwareTickets.ElementType);
            HardwareTickets.GetEnumerator().Returns(hardwareTickets.GetEnumerator());

            TicketResponses.Provider.Returns(ticketResponses.Provider);
            TicketResponses.Expression.Returns(ticketResponses.Expression);
            TicketResponses.ElementType.Returns(ticketResponses.ElementType);
            TicketResponses.GetEnumerator().Returns(ticketResponses.GetEnumerator());

            //create mock context
            MockContext mockContext = Substitute.For<MockContext>();

            //set mock properties
            mockContext.Tickets.Returns(Tickets);
            mockContext.HardwareTickets.Returns(hardwareTickets);
            mockContext.TicketResponses.Returns(TicketResponses);
            #endregion

            return mockContext;
        }


        [TestMethod]
        public void ReadTickets_ReadAllTickets_ReturnsIEnumerableTicketsContainingAllTickets()
        {
            //Arrange
            //inject mock context using constructor
            TicketRepository ticketRepository = new TicketRepository(ctx);

            //Act
            var result = ticketRepository.ReadTickets();

            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Ticket>));
            Assert.AreEqual(result.Count(), ctx.Tickets.Count());
        }
    }
}
