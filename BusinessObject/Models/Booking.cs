using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Booking
{
    public Guid BookingId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
}
