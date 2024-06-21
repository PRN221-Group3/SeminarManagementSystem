using System;
using System.Collections.Generic;

namespace SeminarManagement_PRN221.Models;

public partial class Hall
{
    public Guid HallId { get; set; }

    public string? HallName { get; set; }

    public string? Status { get; set; }

    public int? Capacity { get; set; }

    public string? HallDescription { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
