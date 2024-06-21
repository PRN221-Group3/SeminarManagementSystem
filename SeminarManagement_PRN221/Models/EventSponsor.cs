using System;
using System.Collections.Generic;

namespace SeminarManagement_PRN221.Models;

public partial class EventSponsor
{
    public Guid EventId { get; set; }

    public Guid SponsorId { get; set; }

    public string? Status { get; set; }

    public string? SponsorProduct { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Sponsor Sponsor { get; set; } = null!;
}
