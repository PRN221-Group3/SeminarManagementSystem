using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Sponsor
{
    public Guid SponsorId { get; set; }

    public string? SponsorName { get; set; }

    public string? SponsorType { get; set; }

    public bool? IsDeleted { get; set; }
}
