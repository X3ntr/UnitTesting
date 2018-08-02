using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SC.BL.Domain;
using SC.DAL.EF;

namespace DAL.UnitTests
{
    public class MockContext : SupportCenterDbContext, IMockContext
    {
        public IDbSet<Ticket> Tickets { get; set; }
        public IDbSet<HardwareTicket> HardwareTickets { get; set; }
        public IDbSet<TicketResponse> TicketResponses { get; set; }
    }
}
