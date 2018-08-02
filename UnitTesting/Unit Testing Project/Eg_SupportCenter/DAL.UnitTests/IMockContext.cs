using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SC.BL.Domain;

namespace DAL.UnitTests
{
    public interface IMockContext
    {
        IDbSet<Ticket> Tickets { get; set; }
        IDbSet<HardwareTicket> HardwareTickets { get; set; }
        IDbSet<TicketResponse> TicketResponses { get; set; }
    }
}
