using System;
using System.Collections.Generic;

namespace SeminarManagement_PRN221.Models;

public partial class Booking
{
    public Guid BookingId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
}
