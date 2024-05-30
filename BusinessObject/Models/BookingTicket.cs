using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class BookingTicket
{
    public Guid? TicketId { get; set; }

    public Guid? BookingId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
