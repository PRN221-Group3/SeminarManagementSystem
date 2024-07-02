using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Booking
{
    public Guid BookingId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? TotalTicket { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
