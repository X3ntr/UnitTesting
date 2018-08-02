using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SC.BL.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using SC.DAL.EF;

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
            //Act
            //Assert
        }
    }
}
