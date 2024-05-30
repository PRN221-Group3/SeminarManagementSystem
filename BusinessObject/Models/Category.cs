using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public Guid? TicketId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
