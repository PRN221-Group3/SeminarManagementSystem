using System;
using System.Collections.Generic;

namespace SeminarManagement_PRN221.Models;

public partial class Ticket
{
    public Guid TicketId { get; set; }

    public Guid? EventId { get; set; }

    public Guid? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Event? Event { get; set; }
}
