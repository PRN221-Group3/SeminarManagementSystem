using System;
using System.Collections.Generic;

namespace SeminarManagement_PRN221.Models;

public partial class Sponsor
{
    public Guid SponsorId { get; set; }

    public string? SponsorName { get; set; }

    public string? SponsorType { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<EventSponsor> EventSponsors { get; set; } = new List<EventSponsor>();

    public virtual User SponsorNavigation { get; set; } = null!;
}
