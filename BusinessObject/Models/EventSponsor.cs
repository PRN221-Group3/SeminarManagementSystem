using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class EventSponsor
{
    public Guid? SponsorId { get; set; }

    public Guid? EventId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Sponsor? Sponsor { get; set; }
}
